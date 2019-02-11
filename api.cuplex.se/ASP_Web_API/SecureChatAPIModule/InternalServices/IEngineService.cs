using System;

namespace SecureChatServerModule.InternalServices
{
    public interface IEngineService : IDisposable
    {
        void Start();
        void Stop();
        bool IsRunning { get; }
    }
}
