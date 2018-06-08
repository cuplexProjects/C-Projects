using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using GoogleDDNSService.Managers;
using JetBrains.Annotations;
using NLog;

namespace GoogleDDNSService
{
    [UsedImplicitly]
    public partial class DDNSService : ServiceBase
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger _logger;
        private readonly CoreManager _coreManager;


        public DDNSService(ILifetimeScope lifetimeScope, ILogger logger, CoreManager coreManager)
        {
            _scope = lifetimeScope;
            _logger = logger;
            _coreManager = coreManager;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _logger.Debug("Service started "+DateTime.Now.ToString("g"));

            



        }

        protected override void OnStop()
        {
            _logger.Debug("Service stoped " + DateTime.Now.ToString("g"));
        }
    }
}
