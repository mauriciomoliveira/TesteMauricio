using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Infraestrutura;
using Modelo;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    public class UnidadeTrabalho : IUnidadeTrabalho
    {
        #region Construtores
        static UnidadeTrabalho()
        {
            ISessionFactory fabrica = CriarFabricaSessao(true);
            using (ISession session = fabrica.OpenSession())
            {
                Seed(session);
                session.Close();
            }
        }

        public UnidadeTrabalho()
        {


        }

        #endregion Construtores

        #region IUnidadeTrabalho
        public virtual object Sessao
        {
            get { return (SessaoHibernate); }
        }

        public virtual void Commit(bool criarNovaTransacao = false)
        {
            if (Transacao.IsActive)
                Transacao.Commit();

            if (criarNovaTransacao)
                _transacao = CriarTransacao();

        }

        public virtual void RollBack(bool criarNovaTransacao = false)
        {
            if (Transacao.IsActive)
                Transacao.Rollback();

            if (criarNovaTransacao)
                _transacao = CriarTransacao();
        }

        #endregion IUnidadeTrabalho

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
                    if ((_transacao != null)&&(_transacao.IsActive))
                    {
                        _transacao.Rollback();
                        _transacao.Dispose();
                    }
                    if (_sessaoHibernate != null)
                    {
                        _sessaoHibernate.Close();
                        _sessaoHibernate.Dispose();
                    }
                    if (_fabricaSessao != null)
                        _fabricaSessao.Dispose();
                }
            }
            this.disposed = true;
        }

        #endregion

        protected virtual ISessionFactory FabricaSessao
        {
            get
            {
                if (_fabricaSessao == null)
                {
                    _fabricaSessao = CriarFabricaSessao();
                }
                return _fabricaSessao;
            }
        }
        protected virtual ISession SessaoHibernate
        {
            get
            {
                if (_sessaoHibernate == null)
                {
                    _sessaoHibernate = FabricaSessao.OpenSession();
                }
                return _sessaoHibernate;
            }
        }
        protected virtual ITransaction Transacao
        {
            get
            {
                if (_transacao == null)
                {
                    _transacao = SessaoHibernate.BeginTransaction();
                }
                return _transacao;
            }
        }

        protected static ISessionFactory CriarFabricaSessao(bool recriaBase = false)
        {
            FluentConfiguration configuration = Fluently.Configure()
                .Database(MsSqlCeConfiguration.Standard.ConnectionString(
                   x => x.FromConnectionStringWithKey("baseteste")).ShowSql())
            .Mappings(m => { m.FluentMappings.AddFromAssembly(System.Reflection.Assembly.GetAssembly(typeof(Mapping.CategoriaMapping))); })
            .ExposeConfiguration(c => { new SchemaExport(c).Execute(false, recriaBase, false); });

            //FluentConfiguration configuration = Fluently.Configure()
            //    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(
            //       x => x.FromConnectionStringWithKey("baseteste")).ShowSql())
            //.Mappings(m => { m.FluentMappings.AddFromAssembly(System.Reflection.Assembly.GetAssembly(typeof(Mapping.CategoriaMapping))); })
            //.ExposeConfiguration(c => { new SchemaExport(c).Execute(false, recriaBase, false); });

            return configuration.BuildSessionFactory();
        }

        private static void Seed(ISession session)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                Categoria categoria1 = new Categoria() { Descricao = "Categoria 1" };
                Categoria categoria11 = new Categoria() { Descricao = "Categoria 11", CategoriaPai = categoria1 };
                Categoria categoria12 = new Categoria() { Descricao = "Categoria 12", CategoriaPai = categoria1 };
                Categoria categoria111 = new Categoria() { Descricao = "Categoria 111", CategoriaPai = categoria11 };
                Categoria categoria1111 = new Categoria() { Descricao = "Categoria 1111", CategoriaPai = categoria111 };
                Categoria categoria2 = new Categoria() { Descricao = "Categoria 2" };
                Categoria categoria21 = new Categoria() { Descricao = "Categoria 21", CategoriaPai = categoria2 };
                Categoria categoria22 = new Categoria() { Descricao = "Categoria 22", CategoriaPai = categoria2 };
                Categoria categoria3 = new Categoria() { Descricao = "Categoria 3" };
                session.Save(categoria1);
                session.Save(categoria11);
                session.Save(categoria12);
                session.Save(categoria111);
                session.Save(categoria1111);
                session.Save(categoria2);
                session.Save(categoria21);
                session.Save(categoria22);
                session.Save(categoria3);

                Produto produto1 = new Produto() { Descricao = "Produto 1" };
                Produto produto2 = new Produto() { Descricao = "Produto 2" };
                Produto produto3 = new Produto() { Descricao = "Produto 3" };
                Produto produto4 = new Produto() { Descricao = "Produto 4" };
                Produto produto5 = new Produto() { Descricao = "Produto 5" };

                produto1.Categorias.Add(categoria1);
                produto1.Categorias.Add(categoria2);

                produto2.Categorias.Add(categoria11);

                produto3.Categorias.Add(categoria1);
                produto3.Categorias.Add(categoria11);

                produto4.Categorias.Add(categoria2);
                produto4.Categorias.Add(categoria11);

                session.Save(produto1);
                session.Save(produto2);
                session.Save(produto3);
                session.Save(produto4);
                session.Save(produto5);
                transaction.Commit();
            }
        }

        private ISessionFactory _fabricaSessao = null;

        private ISession _sessaoHibernate = null;

        private ITransaction _transacao = null;

        private ITransaction CriarTransacao()
        {
            return (SessaoHibernate.BeginTransaction());
        }
    }
}
