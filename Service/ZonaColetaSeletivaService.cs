using HoraDoLixo.Model;
using HoraDoLixo.ServiceInterface;
using System.Data.SqlClient;

namespace HoraDoLixo.Service
{
    public class ZonaColetaSeletivaService: IZonaColetaSeletivaService
    {
        private readonly string _connectionString;
        public ZonaColetaSeletivaService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<ZonaColetaSeletiva> GetAll()
        {
            var zonaComum = new List<ZonaColetaSeletiva>();
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand("SELECT * FROM ZonaColetaSeletiva", connection);
            using var read = command.ExecuteReader();
            while (read.Read())
            {
                zonaComum.Add(new ZonaColetaSeletiva
                {
                    IdColetaSeletiva = (int)read["id_zona_coleta_seletiva"],
                    NomeZonaSeletiva = read["nome_zona"].ToString()
                }); ;
            }
            return zonaComum;
        }
    }
}
