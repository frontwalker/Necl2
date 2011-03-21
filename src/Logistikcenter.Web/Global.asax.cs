using System.Web.Mvc;
using System.Web.Routing;
using Logistikcenter.Web.Localization;
using StructureMap;

namespace Logistikcenter.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {        
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Transport", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(Bootstrapper.GetInitializedContainer()));
            ModelMetadataProviders.Current = new ConventionsBasedModelMetaDataProvider();

            Bootstrapper.AddRolesForApplication();
            Bootstrapper.AddAdministratorUser();

            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Application_EndRequest()
        {            
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        }
    }
}