namespace HoraDoLixo.ServiceInterface
{
    public interface IGeocodingService
    {
        Task<(decimal? Latitude, decimal? Longitude)> GeocodeAddressAsync(string address);

    }
}
