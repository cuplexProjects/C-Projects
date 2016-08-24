using System.ServiceProcess;

namespace LoopiaDNSTools
{
    public partial class DomainUpdateService : ServiceBase
    {
        public DomainUpdateService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}