using System.ServiceProcess;
using Autofac;
using GoogleDDNSService.InitService;

namespace GoogleDDNSService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            // First off initialize Corecomponents like Autofac and Logging.
            var container = ApplicationStartup.InitializeSystem();

            using (var scope = container.BeginLifetimeScope())
            {
                var servicesToRun = new ServiceBase[]
                {
                    scope.Resolve<DDNSService>()

                };

                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
