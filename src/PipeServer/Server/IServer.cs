using System;

namespace PipeServer.Server
{
    public interface IServer : IDisposable
    {
        void Start();

        void Send(string messsage);

        void Close();         
    }
}