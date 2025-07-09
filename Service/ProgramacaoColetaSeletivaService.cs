using HoraDoLixo.Model;
using HoraDoLixo.ServiceInterface;
using System.Data.SqlClient;

namespace HoraDoLixo.Service
{
    public class ProgramacaoColetaSeletivaService : IProgramacaoColetaSeletivaService
    {
        private readonly string _connectionString;
        public ProgramacaoColetaSeletivaService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<ProgramacaoColetaSeletiva> GetAll()
        {
            var programacao = new List<ProgramacaoColetaSeletiva>();
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand("SELECT * FROM ProgramacaoColetaSeletiva", connection);
            using var read = command.ExecuteReader();
            while (read.Read())
            {
                programacao.Add(new ProgramacaoColetaSeletiva
                {
                    IdColetaSeletiva = (int)read["id_coleta_seletiva"],
                    IdZonaColetaSeletiva = (int)read["id_zona_coleta_seletiva"],
                    DiaSemana = (int)read["dia_semana"],
                    HorarioInicioPrevisto = (TimeSpan)read["horario_inicio_previsto"],
                    Observacoes = read["observacoes"].ToString()

                }); 
            }
            return programacao;
        }
    }
}
