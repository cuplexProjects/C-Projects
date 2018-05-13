using System.Reflection;
using Autofac;
using FileSystemCore.Configuration;
using FileSystemCore.Managers;
using FileSystemCore.Services;
using FileSystemRules.Configuration;
using Serilog;

namespace FileSystemCore
{
    static class Program
    {
        private static IContainer Container { get; set; }
        private const string FallbackPathpath = @"E:\OneDrive\Users\Win10-Server\Music\";

        static void Main(string[] args)
        {
            //-----------------  Init -------------------------------
            bool debugMode = ApplicationBuildConfig.DebugMode;
            GlobalSettings.Initialize(Assembly.GetExecutingAssembly().GetName().Name, !debugMode);
            InitializeAutofac();

            if (debugMode)
            {
                Log.Verbose("Application started in debug build");
            }
            //-------------------------------------------------------


            using (var scope = Container.BeginLifetimeScope())
            {
                var mainSystem = scope.Resolve<MainSystemService>();
                var commandLineParser = scope.Resolve<CommandLineParser>();
                mainSystem.StartupConfig = commandLineParser.CreateConfigFromInputArgs(args, FallbackPathpath);
                mainSystem.EnterRuntimeEnvironment();
            }

            Log.Verbose("Application ended.");


        }

        private static void InitializeAutofac()
        {
            Container = AutofacConfig.CreateContainer();
        }
    }
}
