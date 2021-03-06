﻿using System.Web.Mvc;
using System.Web.Security;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Logistikcenter.Domain;
using Logistikcenter.Services;
using Logistikcenter.Services.Lingo;
using Logistikcenter.Web.Models;
using Logistikcenter.Web.Persistence;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Logistikcenter.Web
{
    public class Bootstrapper : IBootstrapper
    {
        private bool _ensureContainerIsBootstrapped;

        public void BootstrapStructureMap()
        {
            ObjectFactory.Configure(cfg => cfg.Scan(scanner =>
                                                        {
                                                            scanner.TheCallingAssembly();
                                                            scanner.AddAllTypesOf<IController>().NameBy(c => c.Name.Replace("Controller", "").ToLower());
                                                            scanner.WithDefaultConventions();
                                                            scanner.LookForRegistries();
                                                        }));

            ObjectFactory.AssertConfigurationIsValid();

            _ensureContainerIsBootstrapped = true;
        }

        public IContainer GetContainer()
        {
            if (!_ensureContainerIsBootstrapped)
                BootstrapStructureMap();

            return ObjectFactory.GetInstance<IContainer>();
        }

        public static IContainer GetInitializedContainer()
        {
            var bootstrapper = new Bootstrapper();
            return bootstrapper.GetContainer();
        }

        public static void AddRolesForApplication()
        {
            AddRole("Administrators");
            AddRole("ShippingAgent");            
        }

        private static void AddRole(string roleName)
        {
            if (!Roles.RoleExists(roleName))
                Roles.CreateRole(roleName);
        }

        public static void AddAdministratorUser()
        {
            if (Membership.GetUser("Administrator") == null)
                Membership.CreateUser("Administrator", "Administrator2011", "admin@logistikcenter.se");

            if (!Roles.IsUserInRole("Administrator", "Administrators"))
                Roles.AddUserToRole("Administrator", "Administrators");            
        }
    }

    public class ApplicationRegistry : Registry
    {
        public ApplicationRegistry()
        {
            For<ITransportOptimizationService>().HttpContextScoped().Use<LingoTransportOptimizationService>()
                .Ctor<string>("modelPath").Is(@"C:\Dev\Necl2\src\Logistikcenter.Services\Lingo\Models\prototypCost.lng")
                .Ctor<string>("inputFileFolder").Is(@"C:\Temp")
                .Ctor<string>("logFileFolder").Is(@"C:\Temp\Logs");            
        }
    }

    public class NHibernateRegistry : Registry
    {        
        public NHibernateRegistry()
        {
            ForSingletonOf<ISessionFactory>().Use(BuildSessionFactory());
            For<ISession>().HttpContextScoped().Use(container => container.GetInstance<ISessionFactory>().OpenSession());
            For<IRepository>().Use<NHibernateRepository>();
        }

        static ISessionFactory BuildSessionFactory()
        {
            return Fluently.Configure()
                .Database(MsSqlCeConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey("Logistikcenter")).ShowSql())
                //.Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("Logistikcenter")).ShowSql())
                //.Database(SQLiteConfiguration.Standard.UsingFile(Sqlite).ShowSql)
                //.Database(SQLiteConfiguration.Standard.InMemory)                
                //.Mappings(m => m.FluentMappings.AddFromAssemblyOf<Customer>())                
                .Mappings(m => m.HbmMappings.AddFromAssemblyOf<NHibernateRepository>())
                .ExposeConfiguration(BuildSchema)               
                .BuildSessionFactory();
        }

        static void BuildSchema(Configuration configuration)
        {            
             //new SchemaExport(configuration).Create(false, true);
        }
    }
}