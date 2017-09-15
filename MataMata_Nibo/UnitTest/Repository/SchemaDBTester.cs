using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Framework.Helpers;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace UnitTest.Repository
{
    [TestFixture]
    public class SchemaDBTester
    {
        [Test]
        //[Ignore]
        public void UpdateDatabase()
        {
            NHibernateConfig.Configure(NHibernateConfig.DbOption.Update);
            //NHibernateConfig.Configure(NHibernateConfig.DbOption.Recreate);
        }

        [Test]
        //[Ignore]
        public void GerarSchema()
        {
            FluentConfiguration config = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                .ConnectionString(c => c.FromAppSetting("connectionString")).ShowSql());

            foreach (var assemblyName in AppSettingsHelper.GetValue("mappingAssemblies").Split(','))
            {
                System.Console.WriteLine("carregando assembly " + assemblyName);
                config.Mappings(x => x.FluentMappings.Conventions
                    .Setup(m => m.Add(AutoImport.Never()))
                    .AddFromAssembly(Assembly.Load(assemblyName)));
            }

            config.ExposeConfiguration(BuildSchema);
            config.BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            /* cria script mais nao executa */
            //true,false

            /* Recria o banco de dados */
            //false,true
            new SchemaExport(config).Create(true, false);
        }


    }
}
