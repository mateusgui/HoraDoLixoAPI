using HoraDoLixo.Model;

namespace HoraDoLixo.ServiceInterface
{
    public interface IZonaColetaSeletivaService
    {
        IEnumerable<ZonaColetaSeletiva> GetAll();
    }
}
