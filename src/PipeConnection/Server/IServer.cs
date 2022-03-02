using System;

namespace PipeConnection.Server
{
    public interface IServer : IDisposable
    {
        void Start();

        void Send(string messsage);

        void Close();         
    }
}