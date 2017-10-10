using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Iesi.Collections;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Tool.hbm2ddl;
using Framework.Helpers;
namespace UnitTest.Repository
{
    public static class NHibernateConfig
    {
        public static SessionFactoryAndConfiguration Configure(DbOption option)
        {
            Configuration cfg = null;

            // Configuring NHibernate using Fluent NHibernate.
            FluentConfiguration config = Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2008
                        .ConnectionString(c => c.FromAppSetting("connectionString"))
                        .ShowSql())
                .Cache(c => c.UseQueryCache().ProviderClass<HashtableCacheProvider>());


            foreach (var assemblyName in AppSettingsHelper.GetValue("mappingAssemblies").Split(','))
            {
                System.Console.WriteLine("carregando assembly " + assemblyName);
                config.Mappings(x => x.FluentMappings.Conventions
                    .Setup(m => m.Add(AutoImport.Never()))
                    .AddFromAssembly(Assembly.Load(assemblyName)));
            }

            config.ExposeConfiguration(cfg1 => DbCreateOrUpdate(option, cfg = cfg1));
            var sessionFactory = config.BuildSessionFactory();

           

            return new SessionFactoryAndConfiguration(sessionFactory, cfg);
        }

        public static void DbCreateOrUpdate(DbOption option, Configuration cfg)
        {
            switch (option)
            {
                case DbOption.None:
                    break;
                case DbOption.Update:
                    new SchemaUpdate(cfg).Execute(false, true);
                    break;
                case DbOption.Recreate:
                    new SchemaExport(cfg).Execute(false, true, false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("option");
            }
        }

      
        public class SessionFactoryAndConfiguration
        {
            public SessionFactoryAndConfiguration(ISessionFactory sessionFactory, Configuration configuration)
            {
                this.SessionFactory = sessionFactory;
                this.Configuration = configuration;
            }

            public ISessionFactory SessionFactory { get; private set; }

            public Configuration Configuration { get; private set; }
        }

        public enum DbOption
        {
            None,
            Update,
            Recreate,
        }
    }
}
