using System;
using System.IO;
using System.IO.Pipes;
using PipeClient.Client;

namespace PipeClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
                return;

            IClient client = new MessageClient();
            client.Start(args[0]);
            client.Listen(Console.WriteLine);
        }
    }
}
