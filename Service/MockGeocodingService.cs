using HoraDoLixo.ServiceInterface;

namespace HoraDoLixo.Service
{
    public class MockGeocodingService : IGeocodingService
    {

        /// <summary>
        /// ESTA É UMA IMPLEMENTAÇÃO MOCK (FALSA) PARA DESENVOLVIMENTO.
        /// Ela retorna coordenadas fixas para evitar a necessidade de uma chave de API real.
        /// Em produção, você substituiria esta classe por uma que chama uma API real (Google Maps, OpenStreetMap, etc.).
        /// </summary>
        public Task<(decimal? Latitude, decimal? Longitude)> GeocodeAddressAsync(string address)
        {
            // Retorna coordenadas fixas de exemplo para Campo Grande, MS
            decimal? lat = -20.4697m;
            decimal? lon = -54.6201m;

            // Em uma implementação real, você faria uma chamada HTTP aqui

            return Task.FromResult((lat, lon));
        }
    }
}
