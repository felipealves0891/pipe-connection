﻿using System;
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
            Process pipeClient = new Process();

            pipeClient.StartInfo.FileName = @"D:\Source\Repos\.Net\pipe-connection\src\PipeClient\bin\Debug\netcoreapp3.1\PipeClient.exe";

            using (AnonymousPipeServerStream pipeServer =
                new AnonymousPipeServerStream(PipeDirection.Out,
                HandleInheritability.Inheritable))
            {
                Console.WriteLine("[SERVER] Current TransmissionMode: {0}.",
                    pipeServer.TransmissionMode);

                // Pass the client process a handle to the server.
                pipeClient.StartInfo.Arguments =
                    pipeServer.GetClientHandleAsString();
                pipeClient.StartInfo.UseShellExecute = false;
                pipeClient.Start();

                pipeServer.DisposeLocalCopyOfClientHandle();

                try
                {
                    // Read user input and send that to the client process.
                    using (StreamWriter sw = new StreamWriter(pipeServer))
                    {
                        sw.AutoFlush = true;
                        // Send a 'sync message' and wait for client to receive it.
                        sw.WriteLine("SYNC");
                        pipeServer.WaitForPipeDrain();
                        // Send the console input to the client process.

                        Console.Write("[SERVER] Enter text: ");
                        string text = Console.ReadLine();

                        while(text != null && !text.StartsWith("End"))
                        {
                            sw.WriteLine(text);
                            Thread.Sleep(100);
                            Console.Write("[SERVER] Enter text: ");
                            text = Console.ReadLine();
                        }
                    }
                }
                // Catch the IOException that is raised if the pipe is broken
                // or disconnected.
                catch (IOException e)
                {
                    Console.WriteLine("[SERVER] Error: {0}", e.Message);
                }
            }

            pipeClient.WaitForExit();
            pipeClient.Close();
            Console.WriteLine("[SERVER] Client quit. Server terminating.");
        }
    }
}
