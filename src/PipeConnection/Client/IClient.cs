using System;

namespace PipeConnection.Client
{
    public interface IClient : IDisposable
    {
        void Start(string name);

        void Listen(Action<string> callback); 

        void Close();
    }
}