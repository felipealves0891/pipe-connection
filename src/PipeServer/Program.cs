using PipeServer.Server;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace PipeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string executable = @"D:\Source\Repos\.Net\pipe-connection\src\PipeClient\bin\Debug\netcoreapp3.1\PipeClient.exe";
            using (IServer server = new MessageServer(executable))
            {
                server.Start();

                for(int i = 0; i < 100; i++)
                {
                    server.Send($"Loop '{i}'!");
                    Thread.Sleep(100);
                }
            }
        }
    }
}
