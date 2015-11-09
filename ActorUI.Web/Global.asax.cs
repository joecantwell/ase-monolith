
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ActorUI.Actors;
using ActorUI.Web.App_Start;
using Akka.Actor;

namespace ActorUI.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.RegisterMappings();

            ActorSystemRefs.ActorSystem = ActorSystem.Create("BrokerInsuranceSystem");
            var actorSystem = ActorSystemRefs.ActorSystem;

            //register toplevel actor(s)
            SystemActors.VehicleActor = actorSystem.ActorOf(Props.Create(typeof(VehicleActor)), "FindMyCarActor");          
            SystemActors.QuoteActor = actorSystem.ActorOf(Props.Create(typeof(QuoteCoordinatorActor)), "InsuranceQuote");
        }

        protected void Application_End()
        {
            ActorSystemRefs.ActorSystem.Shutdown();
            ActorSystemRefs.ActorSystem.AwaitTermination(TimeSpan.FromSeconds(2)); // wait for a clean shutdown!
        }

    }
}
