using System.Collections.Generic;
using System.Configuration;
using Broker.Persistance;
using Broker.Service.Contracts;
using Broker.Services;
using BrokerUI.web.Configuration;
using Ninject.Extensions.Conventions;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(BrokerUI.web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(BrokerUI.web.App_Start.NinjectWebCommon), "Stop")]

namespace BrokerUI.web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        private static readonly string _environment = ConfigurationManager.AppSettings["Environment"] ?? "Production";

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // Misc
            kernel.Bind<Entities>().To<Entities>();

            switch (_environment)
            {
                case "Debug":
                    kernel.Bind<ISystemConfiguration>().To<DebugConfiguration>().InRequestScope();
                    break;
                default: // default to production
                    kernel.Bind<ISystemConfiguration>().To<ProductionConfiguration>().InRequestScope();
                    break;
            }


            // using nInject conventions plug-in 
            // https://github.com/ninject/Ninject.Extensions.Conventions/wiki
            // it takes all dlls with matching pattern and binds interfaces
            // like ISomeClass to SomeClass
            // if for any reason we need to exclude an object - please add it to ExcludeList method

            kernel.Bind(x => x
                .FromAssembliesMatching("Broker*.dll")
                .SelectAllClasses() 
                .Excluding(ExcludeList())
                .BindDefaultInterface()
                .Configure(b => b.InRequestScope()));

        }

        /// <summary>
        /// List of object to exclude in ninject convention binding
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Type> ExcludeList()
        {
            var types = new List<Type>
            {
                typeof (ISystemConfiguration)
            };

            return types;
        }       
    }
}
