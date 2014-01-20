using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Infraestrutura;
using Modelo;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    public class RepositorioBase<T> : IRepositorio<T>, IDisposable where T : class
    {
        private IUnidadeTrabalho _unidadeTrabalho = null;

        protected virtual ISession Sessao
        {
            get
            {
                if (UnidadeTrabalho.Sessao is ISession)
                    return ((ISession)UnidadeTrabalho.Sessao);
                else
                    throw (new Exception("Tipo da instância de sessão inválida para este repositório.\r\nRequerida sessão do nHibernate."));
            }
        }

        public virtual IUnidadeTrabalho UnidadeTrabalho
        {
            get { return _unidadeTrabalho; }
            set { _unidadeTrabalho = value; }
        }

        public RepositorioBase(IUnidadeTrabalho unidadeTrabalho)
        {

        }

        public virtual IList<T> Listar()
        {
            return (Sessao.CreateCriteria(typeof(T)).List<T>());
        }

        public virtual T Atualizar(T entidade)
        {
            using (ITransaction transaction = Sessao.BeginTransaction())
            {
                Sessao.Update(entidade);
                transaction.Commit();
            }
            return (entidade);
        }

        public virtual T Obter(int id)
        {
            return (Sessao.CreateCriteria<T>().Add(Restrictions.Eq("Id", id)).UniqueResult<T>());
        }

        public virtual T Criar(T entidade)
        {
            using (ITransaction transaction = Sessao.BeginTransaction())
            {
                Sessao.Save(entidade);
                transaction.Commit();
            }
            return (entidade);
        }

        public virtual void Remover(T entidade)
        {
            using (ITransaction transaction = Sessao.BeginTransaction())
            {
                Sessao.Delete(entidade);
                transaction.Commit();
            }
        }

        #region IDisposable

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }
            }
            this.disposed = true;
        }

        #endregion
    }
}
