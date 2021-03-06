﻿
using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ActorUI.Actors;
using ActorUI.Web.App_Start;
using Akka.Actor;
using Broker.Persistance;

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

            try
            {
                ActorSystemRefs.ActorSystem = ActorSystem.Create("BrokerInsuranceSystem");
                var actorSystem = ActorSystemRefs.ActorSystem;

                //register toplevel actor(s)
                SystemActors.VehicleActor = actorSystem.ActorOf(Props.Create(() => new VehicleActor(new Entities())), "FindMyCarActor");          
                SystemActors.QuoteActor = actorSystem.ActorOf(Props.Create(() => new QuoteCoordinatorActor(new Entities())), "InsuranceQuote");
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
           
        }

        protected void Application_End()
        {
            ActorSystemRefs.ActorSystem.Shutdown();
            ActorSystemRefs.ActorSystem.AwaitTermination(TimeSpan.FromSeconds(2)); // wait for a clean shutdown!
        }  

    }
}
