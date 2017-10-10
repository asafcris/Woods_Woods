using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Cache;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Framework.Helpers;

namespace Framework.NHibernate
{
    public sealed class NHibernateHelper
    {
        #region Thread-safe, lazy Singleton

        public static NHibernateHelper Instance
        {
            get
            {
                return Nested.NHibernateSessionManager;
            }
        }

        private NHibernateHelper()
        {
            InitSessionFactory();
        }

        private class Nested
        {
            static Nested() { }
            internal static readonly NHibernateHelper NHibernateSessionManager = new NHibernateHelper();
        }

        #endregion

        private void InitSessionFactory()
        {

            string databaseType = AppSettingsHelper.GetValue("databaseType");

            if (string.IsNullOrEmpty(databaseType))
            {
                databaseType = "MsSql2008";
            }

            FluentConfiguration config = Fluently.Configure();

            switch (databaseType) {
                case "MySql":
                    config.Database(MySQLConfiguration.Standard.ConnectionString(c => c.FromAppSetting("connectionString")).ShowSql());
                    break;
                case "MsSql2008":
                    config.Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromAppSetting("connectionString")).ShowSql());
                    break;
                case "MsSql2012":
                    config.Database(MsSqlConfiguration.MsSql2012.ConnectionString(c => c.FromAppSetting("connectionString")).ShowSql());
                    break;
            }

            foreach (var assemblyName in AppSettingsHelper.GetValue("mappingAssemblies").Split(','))
                config.Mappings(x => x.FluentMappings.Conventions.Setup(m => m.Add(AutoImport.Never())).AddFromAssembly(Assembly.Load(assemblyName)));

            sessionFactory = config.BuildSessionFactory();

        }

        public void RegisterInterceptor(IInterceptor interceptor)
        {
            ISession session = ContextSession;

            if (session != null && session.IsOpen)
            {
                throw new CacheException("You cannot register an interceptor once a session has already been opened");
            }

            GetSession(interceptor);
        }

        public ISession GetSession()
        {
            return GetSession(null);
        }

        private ISession GetSession(IInterceptor interceptor)
        {
            ISession session = ContextSession;

            if (session == null)
            {
                if (interceptor != null)
                {
                    session = sessionFactory.OpenSession(interceptor);
                }
                else
                {
                    session = sessionFactory.OpenSession();
                }

                ContextSession = session;
            }

            return session;
        }

        public void CloseSession()
        {
            ISession session = ContextSession;

            if (session != null && session.IsOpen)
            {
                session.Flush();
                session.Close();
            }

            ContextSession = null;
        }

        public void BeginTransaction()
        {
            ITransaction transaction = ContextTransaction;

            if (transaction == null)
            {
                transaction = GetSession().BeginTransaction();
                ContextTransaction = transaction;
            }
        }

        public void CommitTransaction()
        {
            ITransaction transaction = ContextTransaction;

            try
            {
                if (HasOpenTransaction())
                {
                    transaction.Commit();
                    ContextTransaction = null;
                }
            }
            catch (HibernateException)
            {
                RollbackTransaction();
                throw;
            }
        }

        public bool HasOpenTransaction()
        {
            ITransaction transaction = ContextTransaction;

            return transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack;
        }

        public void RollbackTransaction()
        {
            ITransaction transaction = ContextTransaction;

            try
            {
                if (HasOpenTransaction())
                {
                    transaction.Rollback();
                }

                ContextTransaction = null;
            }
            finally
            {
                CloseSession();
            }
        }

        private ITransaction ContextTransaction
        {
            get
            {
                return (ITransaction)CallContext.GetData(TRANSACTION_KEY);
            }
            set
            {
                CallContext.SetData(TRANSACTION_KEY, value);
            }
        }

        private ISession ContextSession
        {
            get
            {
                return (ISession)CallContext.GetData(SESSION_KEY);
            }
            set
            {
                CallContext.SetData(SESSION_KEY, value);
            }
        }

        private const string TRANSACTION_KEY = "CONTEXT_TRANSACTION";
        private const string SESSION_KEY = "CONTEXT_SESSION";
        private ISessionFactory sessionFactory;
    }
}
