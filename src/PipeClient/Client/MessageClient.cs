using System;
using System.IO;
using System.IO.Pipes;

namespace PipeClient.Client
{
    public class MessageClient : IClient
    {
        private StreamReader _client;

        private bool _initialized;

        public MessageClient()
        {
            _initialized = false;
        }

        public void Start(string name)
        {
            try
            {
                PipeStream pipe 
                    = new AnonymousPipeClientStream(PipeDirection.In, name);
                
                _client = new StreamReader(pipe);
                _initialized = true;
            }
            catch(Exception ex)
            {
                _initialized = false;
                throw ex;
            } 
        }

        public void Listen(Action<string> callback)
        {
            if(!_initialized)
                return;

            string temp;
            while ((temp = _client.ReadLine()) != null)
            {
                if(callback != null)
                    callback(temp);
            }
        }

        public void Dispose()
        {
            if(_initialized)
            {
                _client.Dispose();
                _initialized = false;         
            }
        }
    }
}