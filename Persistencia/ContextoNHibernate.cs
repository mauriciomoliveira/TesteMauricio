using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    public class ContextoNHibernate : IDisposable
    {
        #region Campos

        private FluentConfiguration _fluentConfig = null;

        private NHibernate.Cfg.Configuration _nHibernateConfig = null;

        private string _nameOrConnectionString;

        private ISessionFactory _sessionFactory;

        private ISession _session = null;

        private ITransaction _transaction = null;

        #endregion

        #region Propriedades

        private ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = CreateSessionFactory();
                }
                return _sessionFactory;
            }
        }

        protected FluentConfiguration FluentConfig
        {
            get { return _fluentConfig; }
            set { _fluentConfig = value; }
        }

        protected NHibernate.Cfg.Configuration NHibernateConfig
        {
            get { return _nHibernateConfig; }
            set { _nHibernateConfig = value; }
        }

        public ISession Session
        {
            get
            {
                if (_session == null)
                {
                    _session = OpenSession();
                }
                return _session;

            }
        }

        internal ITransaction Transaction
        {
            get
            {
                if (_transaction == null)
                {
                    _transaction = this.Session.BeginTransaction();
                }
                return _transaction;
            }
            set
            {

                if (_transaction != value)
                {
                    if (value == null)
                    {
                        if (_transaction.IsActive)
                            _transaction.Rollback();
                        _transaction.Dispose();
                    }
                    _transaction = value;
                }

            }
        }

        #endregion

        #region Construtores

        public ContextoNHibernate()
        {
            Configurar();
        }

        #endregion

        public ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        #region IContexto

        #region Virtuais

        public virtual void Configurar()
        {
            this.FluentConfig = Fluently.Configure();
            System.Reflection.Assembly mappingAssembly = System.Reflection.Assembly.GetAssembly(typeof(Mapping.CategoriaMapping));//.Load(ConfigurationManager.AppSettings["mappingAssembly"]);
            this.FluentConfig = this.FluentConfig.Mappings(m => { m.FluentMappings.AddFromAssembly(mappingAssembly); });
            this.FluentConfig = this.FluentConfig.ExposeConfiguration(BuildSchema);
            DefinirInicializador();
        }
        public virtual void Save()
        {
            if (_transaction != null)
            {
                if (_transaction.IsActive)
                    _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public virtual void DefinirInicializador()
        {
        }

        #endregion

        #endregion IContexto

        #region Metodos Restritos

        private ISessionFactory CreateSessionFactory()
        {
            ISessionFactory sessionFactory = this.FluentConfig.BuildSessionFactory();
            return sessionFactory;
        }

        protected virtual void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            this.NHibernateConfig = config;
            new SchemaExport(config).SetOutputFile("E:\\Projetos\\Mauricio\\EsquemaTeste.sql").Execute(false, false, false);
            //new SchemaUpdate(config).Execute(true, true);
        }

        #endregion

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
                    if (_transaction != null)
                    {
                        if (_transaction.IsActive)
                            _transaction.Rollback();
                        _transaction.Dispose();
                        _transaction = null;
                    }

                    if (_session != null)
                    {
                        _session.Disconnect();
                        _session.Dispose();
                        _session = null;
                    }
                }
            }
            this.disposed = true;
        }

        #endregion
    }
}
