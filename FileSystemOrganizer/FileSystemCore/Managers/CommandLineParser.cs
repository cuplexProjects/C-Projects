using System.Reflection;
using AutoMapper;
using FileSystemCore.Models;
using JetBrains.Annotations;
using Serilog;

namespace FileSystemCore.Managers
{
    [UsedImplicitly]
    public class CommandLineParser : ManagerBase
    {
        private readonly IMapper _mapper;

        public CommandLineParser(IMapper mapper)
        {
            _mapper = mapper;
        }

        public StartupConfigModel CreateConfigFromInputArgs(string[] args, string fallbackPath)
        {
            var conf = new StartupConfigModel();

            conf.ExecutingPath = Assembly.GetCallingAssembly().Location;

            if (args.Length == 0)
            {
                Log.Warning(ResourceMessages.Startup_Command_Empty);
                conf.BasePathRequested = fallbackPath;
            }



            return conf;
        }
    }
}
