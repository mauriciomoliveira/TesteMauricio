using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura
{
    public interface IUnidadeTrabalho : IDisposable
    {
        Object Sessao { get; }
        void Commit(bool criarNovaTransacao = false);
        void RollBack(bool criarNovaTransacao = false);
    }
}
