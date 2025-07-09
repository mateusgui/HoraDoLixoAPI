using HoraDoLixo.Model;

namespace HoraDoLixo.ServiceInterface
{
    public interface IProgramacaoColetaComumService
    {
        IEnumerable<ProgramacaoColetaComum> GetAll();
    }
}
