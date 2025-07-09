using HoraDoLixo.Model;
using HoraDoLixo.ServiceInterface;
using System.Data.SqlClient;

namespace HoraDoLixo.Service
{
    public class ZonaColetaComumService : IZonaColetaComumService
    {
        private readonly string _connectionString;
        public ZonaColetaComumService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<ZonaColetaComum> GetAll()
        {
            var zonaComum = new List<ZonaColetaComum>();
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand("SELECT * FROM ZonaColetaComum", connection);
            using var read = command.ExecuteReader();
            while (read.Read())
            {
                zonaComum.Add(new ZonaColetaComum
                {
                    IdColetaComum = (int)read["id_zona_coleta_comum"],
                    NomeZonaComum = read["nome_zona"].ToString()
                }); ;
            }
            return zonaComum;
        }
    }
}
