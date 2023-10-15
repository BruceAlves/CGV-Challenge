using Dominio;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IAdvogadoRepositorio
    {
        IEnumerable<Advogado> ListarAdvogadoComEndereco();

        void Incluir(Advogado pObjadvogado);

        void Atualizar(Advogado pObjadvogado);

        void Excluir(int pIntadvogado);

        Advogado BuscarPorId(int pIntadvogado);
    }
}
