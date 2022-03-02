using System;

namespace PipeClient.Client
{
    public interface IClient : IDisposable
    {
        void Start(string name);

        void Listen(Action<string> callback); 
    }
}