using System;

namespace SecureChat.Adapter.Server.Features
{
    public interface IHostFeature : IDisposable
    {
        void Setup();
        void Stop();
    }
}