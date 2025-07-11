﻿using HoraDoLixo.Dto;
using HoraDoLixo.Model;
using HoraDoLixo.ServiceInterface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HoraDoLixo.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly IGeocodingService _geocodingService;

        public UsuarioService(IConfiguration configuration, IGeocodingService geocodingService)
        {
            _configuration = configuration;
            _geocodingService = geocodingService;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Usuario> CreateAsync(UsuarioCreateDto usuarioDto)
        {

            using (var checkConnection = new NpgsqlConnection(_connectionString))
            {
                await checkConnection.OpenAsync();

                var checkCommand = new NpgsqlCommand("SELECT COUNT(1) FROM \"usuario\" WHERE email = @Email", checkConnection);
                checkCommand.Parameters.AddWithValue("@Email", usuarioDto.Email);
                if (Convert.ToInt64(await checkCommand.ExecuteScalarAsync()) > 0)
                {
                    throw new InvalidOperationException("O e-mail informado já está em uso.");
                }
            }

            string enderecoCompleto = $"{usuarioDto.EnderecoRua}, {usuarioDto.EnderecoNumero}, {usuarioDto.EnderecoBairro}, Campo Grande, MS, {usuarioDto.Cep}";
            var (latitude, longitude) = await _geocodingService.GeocodeAddressAsync(enderecoCompleto);

            var novoUsuario = new Usuario
            {
                NomeCompleto = usuarioDto.NomeCompleto,
                Email = usuarioDto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Senha),
                Telefone = usuarioDto.Telefone,
                EnderecoRua = usuarioDto.EnderecoRua,
                EnderecoNumero = usuarioDto.EnderecoNumero,
                EnderecoComplemento = usuarioDto.EnderecoComplemento,
                EnderecoBairro = usuarioDto.EnderecoBairro,
                Cep = usuarioDto.Cep,
                Latitude = latitude,
                Longitude = longitude,
                DataCadastro = DateTime.UtcNow,
                Status = true,
                AlertaComumAtivo = true,
                AlertaSeletivaAtivo = true
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"INSERT INTO ""usuario"" (nome_completo, email, senha_hash, telefone, endereco_rua, endereco_numero, endereco_complemento, endereco_bairro, cep, latitude, longitude, data_cadastro, status, alerta_comum_ativo, alerta_seletiva_ativo)
                            VALUES (@NomeCompleto, @Email, @SenhaHash, @Telefone, @EnderecoRua, @EnderecoNumero, @EnderecoComplemento, @EnderecoBairro, @Cep, @Latitude, @Longitude, @DataCadastro, @Status, @AlertaComumAtivo, @AlertaSeletivaAtivo)
                            RETURNING id_usuario;";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NomeCompleto", novoUsuario.NomeCompleto);
                    command.Parameters.AddWithValue("@Email", novoUsuario.Email);
                    command.Parameters.AddWithValue("@SenhaHash", novoUsuario.SenhaHash);
                    command.Parameters.AddWithValue("@Telefone", (object)novoUsuario.Telefone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EnderecoRua", (object)novoUsuario.EnderecoRua ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EnderecoNumero", (object)novoUsuario.EnderecoNumero ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EnderecoComplemento", (object)novoUsuario.EnderecoComplemento ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EnderecoBairro", (object)novoUsuario.EnderecoBairro ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Cep", (object)novoUsuario.Cep ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Latitude", (object)novoUsuario.Latitude ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Longitude", (object)novoUsuario.Longitude ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DataCadastro", novoUsuario.DataCadastro);
                    command.Parameters.AddWithValue("@Status", novoUsuario.Status);
                    command.Parameters.AddWithValue("@AlertaComumAtivo", novoUsuario.AlertaComumAtivo);
                    command.Parameters.AddWithValue("@AlertaSeletivaAtivo", novoUsuario.AlertaSeletivaAtivo);

                    novoUsuario.IdUsuario = (int)await command.ExecuteScalarAsync();
                }
            }
            return novoUsuario;
        }

        public async Task<bool> UpdateAsync(int id, UsuarioUpdateDto dto)
        {
            var usuarioAtual = GetById(id);
            if (usuarioAtual == null)
            {
                return false;
            }

            decimal? novaLatitude = usuarioAtual.Latitude;
            decimal? novaLongitude = usuarioAtual.Longitude;

            bool enderecoMudou =
                (dto.EnderecoRua != null && dto.EnderecoRua != usuarioAtual.EnderecoRua) ||
                (dto.EnderecoNumero != null && dto.EnderecoNumero != usuarioAtual.EnderecoNumero) ||
                (dto.EnderecoBairro != null && dto.EnderecoBairro != usuarioAtual.EnderecoBairro) ||
                (dto.Cep != null && dto.Cep != usuarioAtual.Cep);

            if (enderecoMudou)
            {
                string enderecoCompleto = $"{dto.EnderecoRua ?? usuarioAtual.EnderecoRua}, {dto.EnderecoNumero ?? usuarioAtual.EnderecoNumero}, {dto.EnderecoBairro ?? usuarioAtual.EnderecoBairro}, Campo Grande, MS, {dto.Cep ?? usuarioAtual.Cep}";
                var (latitude, longitude) = await _geocodingService.GeocodeAddressAsync(enderecoCompleto);
                novaLatitude = latitude;
                novaLongitude = longitude;
            }

            var setClauses = new List<string>();
            var parameters = new List<NpgsqlParameter>();

            Action<string, object> addClause = (columnName, value) =>
            {
                if (value != null)
                {
                    var paramName = $"@{columnName}";
                    setClauses.Add($"\"{columnName}\" = {paramName}");
                    parameters.Add(new NpgsqlParameter(paramName, value));
                }
            };

            addClause("telefone", dto.Telefone);
            addClause("horario_alerta_comum", dto.HorarioAlertaComum);
            addClause("alerta_comum_ativo", dto.AlertaComumAtivo);
            addClause("horario_alerta_seletiva", dto.HorarioAlertaSeletiva);
            addClause("alerta_seletiva_ativo", dto.AlertaSeletivaAtivo);
            addClause("endereco_rua", dto.EnderecoRua);
            addClause("endereco_numero", dto.EnderecoNumero);
            addClause("endereco_complemento", dto.EnderecoComplemento);
            addClause("endereco_bairro", dto.EnderecoBairro);
            addClause("cep", dto.Cep);

            if (enderecoMudou)
            {
                addClause("latitude", novaLatitude);
                addClause("longitude", novaLongitude);
            }

            if (!setClauses.Any())
            {
                return true;
            }

            var sql = $"UPDATE \"usuario\" SET {string.Join(", ", setClauses)} WHERE id_usuario = @IdUsuario";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    command.Parameters.AddWithValue("@IdUsuario", id);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<(Usuario? usuario, string? token)> LoginAsync(LoginDto loginDto)
        {
            var usuario = await GetByEmailAsync(loginDto.Email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, usuario.SenhaHash))
            {
                return (null, null);
            }

            var token = GenerateJwtToken(usuario);
            return (usuario, token);
        }

        public IEnumerable<Usuario> GetAll()
        {
            var usuarios = new List<Usuario>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("SELECT * FROM \"usuario\"", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarios.Add(MapToUsuario(reader));
                    }
                }
            }
            return usuarios;
        }

        public Usuario? GetById(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("SELECT * FROM \"usuario\" WHERE id_usuario = @IdUsuario", connection);
                command.Parameters.AddWithValue("@IdUsuario", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapToUsuario(reader);
                    }
                }
            }
            return null;
        }

        private async Task<Usuario?> GetByEmailAsync(string email)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new NpgsqlCommand("SELECT * FROM \"usuario\" WHERE email = @Email", connection);
            command.Parameters.AddWithValue("@Email", email);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return MapToUsuario(reader);
            return null;
        }

        public async Task<InformacoesColetaDto?> GetInformacoesColetaAsync(int id)
        {
            var usuario = GetById(id);
            if (usuario == null || !usuario.Latitude.HasValue || !usuario.Longitude.HasValue)
            {
                return null;
            }

            var informacoes = new InformacoesColetaDto
            {
                ColetaComum = await GetZonaInfoByCoordsAsync("Comum", usuario.Latitude.Value, usuario.Longitude.Value),
                ColetaSeletiva = await GetZonaInfoByCoordsAsync("Seletiva", usuario.Latitude.Value, usuario.Longitude.Value)
            };
            return informacoes;
        }

        private async Task<ZonaColetaInfoDto?> GetZonaInfoByCoordsAsync(string tipo, decimal latitude, decimal longitude)
        {
            // TODO: A query original era específica do SQL Server e precisa ser reescrita

            await Task.CompletedTask;
            return null;
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("Chave JWT não configurada."));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private Usuario MapToUsuario(NpgsqlDataReader reader)
        {
            return new Usuario
            {
                IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                NomeCompleto = reader.GetString(reader.GetOrdinal("nome_completo")),
                Email = reader.GetString(reader.GetOrdinal("email")),
                SenhaHash = reader.GetString(reader.GetOrdinal("senha_hash")),
                Telefone = reader.IsDBNull(reader.GetOrdinal("telefone")) ? null : reader.GetString(reader.GetOrdinal("telefone")),
                EnderecoRua = reader.IsDBNull(reader.GetOrdinal("endereco_rua")) ? null : reader.GetString(reader.GetOrdinal("endereco_rua")),
                EnderecoNumero = reader.IsDBNull(reader.GetOrdinal("endereco_numero")) ? null : reader.GetString(reader.GetOrdinal("endereco_numero")),
                EnderecoComplemento = reader.IsDBNull(reader.GetOrdinal("endereco_complemento")) ? null : reader.GetString(reader.GetOrdinal("endereco_complemento")),
                EnderecoBairro = reader.IsDBNull(reader.GetOrdinal("endereco_bairro")) ? null : reader.GetString(reader.GetOrdinal("endereco_bairro")),
                Cep = reader.IsDBNull(reader.GetOrdinal("cep")) ? null : reader.GetString(reader.GetOrdinal("cep")),
                Latitude = reader.IsDBNull(reader.GetOrdinal("latitude")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("latitude")),
                Longitude = reader.IsDBNull(reader.GetOrdinal("longitude")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("longitude")),
                DataCadastro = reader.GetDateTime(reader.GetOrdinal("data_cadastro")),
                Status = reader.GetBoolean(reader.GetOrdinal("status")),
                AlertaComumAtivo = reader.GetBoolean(reader.GetOrdinal("alerta_comum_ativo")),
                AlertaSeletivaAtivo = reader.GetBoolean(reader.GetOrdinal("alerta_seletiva_ativo")),
                HorarioAlertaComum = reader.IsDBNull(reader.GetOrdinal("horario_alerta_comum")) ? (TimeSpan?)null : reader.GetTimeSpan(reader.GetOrdinal("horario_alerta_comum")),
                HorarioAlertaSeletiva = reader.IsDBNull(reader.GetOrdinal("horario_alerta_seletiva")) ? (TimeSpan?)null : reader.GetTimeSpan(reader.GetOrdinal("horario_alerta_seletiva"))
            };
        }
    }
}