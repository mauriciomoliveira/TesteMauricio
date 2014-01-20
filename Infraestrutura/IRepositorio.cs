using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura
{
    public interface IRepositorio<T> : IDisposable where T : class
    {
        T Atualizar(T entidade);
        IList<T> Listar();
        T Obter(int id);
        T Criar(T entidade);
        void Remover(T entidade);

        IUnidadeTrabalho UnidadeTrabalho { get; }

    }
}
