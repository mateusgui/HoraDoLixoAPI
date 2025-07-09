using HoraDoLixo.Model;
using HoraDoLixo.ServiceInterface;
using System.Data.SqlClient;

namespace HoraDoLixo.Service
{
    public class ProgramacaoColetaComumService : IProgramacaoColetaComumService
    {
        private readonly string _connectionString;
        public ProgramacaoColetaComumService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<ProgramacaoColetaComum> GetAll()
        {
            var zonaComum = new List<ProgramacaoColetaComum>();
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand("SELECT * FROM ProgramacaoColetaComum", connection);
            using var read = command.ExecuteReader();
            while (read.Read())
            {
                zonaComum.Add(new ProgramacaoColetaComum
                {
                    IdColetaComum = (int)read["id_coleta_comum"],
                    IdZonaColetaComum = (int)read["id_zona_coleta_comum"],
                    DiaSemana = (int)read["dia_semana"],
                    HorarioInicioPrevisto = (TimeSpan)read["horario_inicio_previsto"],
                    Observacoes = read["observacoes"].ToString()
                }); ;
            }
            return zonaComum;
        }
    }
}
