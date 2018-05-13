using System;
using AutoMapper;
using FileSystemCore.DataModels;
using FileSystemCore.Models;
using FileSystemCore.Repositories;
using JetBrains.Annotations;
using Serilog;

namespace FileSystemCore.Services
{
    [UsedImplicitly]
    public class MainSystemService : ServiceBase
    {
        private readonly IMapper _mapper;
        private readonly DictionaryDocRepository _docRepository;

        public StartupConfigModel StartupConfig { get; set; }

        public ApplicationState CurrentState { get; private set; }

        public MainSystemService(IMapper mapper, DictionaryDocRepository docRepository)
        {
            CurrentState = ApplicationState.Initializing;
            _mapper = mapper;
            _docRepository = docRepository;
        }


        /// <summary>
        /// Main application loop
        /// Applications the runtime environment.
        /// </summary>
        public void EnterRuntimeEnvironment()
        {
            CurrentState = ApplicationState.Running;

            if (StartupConfig == null)
            {
                Log.Error("No configuration was set from Main Program entry point, termenating application");
                Console.ReadLine();
                return;
            }

            while (CurrentState == ApplicationState.Running)
            {


                string consoleReadLine = Console.ReadLine();
            }
        }


    }
}
