[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ChatStatsApi.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(ChatStatsApi.App_Start.NinjectWebCommon), "Stop")]

namespace ChatStatsApi.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Pojito.Azure.Storage.Table;
    using System.Configuration;
    using AzureStorage;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

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
            //var connectionString = "DefaultEndpointsProtocol=https;AccountName=sachatdata;AccountKey=AgG/k5DWKBIV3ZpMlnFc/wy8ZimhGhJirufXoyD0EXYZA/9giZSzXCLX1w/+qR7l/FEIWRHB5HK5VbxmqArwfg==;";
            //var connectionString = "UseDevelopmentStorage=true";
            var connectionString = ConfigurationManager.AppSettings["StorageAccountConnectionString"];

            kernel.Bind<IMessageRepository>().To<MessageRepository>();

            kernel.Bind<StorageFactory>().To<StorageFactory>()
                .WithConstructorArgument("connectionString", connectionString);

        }        
    }
}
