using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

namespace PipeConnection.Server
{
    public class MessageServer : IServer
    {
        private readonly AnonymousPipeServerStream _pipe;

        private readonly Process _client;

        private StreamWriter _writer;
        
        private bool _initiated;

        public MessageServer(string executable)
        {
            _pipe = new AnonymousPipeServerStream(PipeDirection.Out,
                HandleInheritability.Inheritable);

            _client = new Process();
            _client.StartInfo.FileName = executable;
            _client.StartInfo.Arguments = _pipe.GetClientHandleAsString();
            _client.StartInfo.UseShellExecute = false;
            _initiated = false;
        }

        public void Start()
        {
            try
            {
                _client.Start();
                // Send a 'sync message' and wait for client to receive it.
                _pipe.DisposeLocalCopyOfClientHandle();
                _writer = new StreamWriter(_pipe);
                _writer.AutoFlush = true;
                _writer.WriteLine("SYNC");
                _pipe.WaitForPipeDrain();
                _initiated = true;
            }
            catch(Exception ex)
            {
                _initiated = false;
                throw ex;
            }
        }

        public void Send(string message)
        {
            _writer.WriteLine(message);
            _pipe.WaitForPipeDrain();
        }
        
        public void Close()
        {
            if(_initiated)
            {
                _client.Close();
                _writer.Close();
                _initiated = false;
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}