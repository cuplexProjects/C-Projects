using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NLog;

namespace GoogleDDNSService.Managers
{
    [UsedImplicitly]
    public class CoreManager : ManagerBase
    {
        private readonly ILogger _logger;
        private readonly NetworkDataManager _networkDataManager;
        private Task _mainTask;
        private readonly TaskFactory _taskFactory;
        private readonly SemaphoreSlim _semaphoreSlim;
        private Timer _timer;

        public bool IsStarted { get; private set; }
        public bool IsRunning { get; private set; }



        private delegate void ManagementLoop();

        public CoreManager(ILogger logger, NetworkDataManager networkDataManager)
        {
            _logger = logger;
            _networkDataManager = networkDataManager;
            _taskFactory = new TaskFactory(TaskScheduler.Default);
            _timer = new Timer(CoreManagerTimer, null, 250, 300000);

        }

        public void Start()
        {
            if (IsStarted && IsRunning)
            {
                return;
            }

            IsStarted = true;

            if (!IsRunning)
            {
                IsRunning = true;

            }
        }

        public void Stop()
        {
            IsRunning = false;

            if (_mainTask != null && !_mainTask.IsCompleted && !_mainTask.Wait(1000))
            {
                _taskFactory.CancellationToken.WaitHandle.SafeWaitHandle.Close();

            }

        }


        private void CoreManagerTimer(object state)
        {

        }

    }
}