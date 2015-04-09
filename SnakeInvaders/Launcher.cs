using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NetworkEngine;
using SnakeInvaderAi;

namespace SnakeInvaders
{
    public class Launcher
    {
        private readonly string _host;
        private readonly int _port;
        private TcpWriter _writer;
        private TcpReader _reader;
        private CancellationTokenSource _cancel;
        private Pipe<object> _pipe;
        private NibblesAiHandler _aiHandler;
        private INibblesAi _ai;

        public Launcher(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public void Go()
        {
            using (var tcpClient = new TcpClient())
            {
                tcpClient.NoDelay = true;
                tcpClient.Connect(_host, _port);

                var stream = tcpClient.GetStream();
                _cancel = new CancellationTokenSource();
                _pipe = new Pipe<dynamic>();

                _writer = new TcpWriter(stream, _cancel.Token, _pipe);
                _reader = new TcpReader(stream, _cancel.Token, _pipe);

                _ai = new NibblesAi();
                _aiHandler = new NibblesAiHandler(_ai, _pipe.ReceiveQueue, _pipe.SendQueue, _cancel.Token);
                
                var tasks = new List<Task>();
                tasks.Add(Task.Factory.StartNew(() => _aiHandler.Go(), _cancel.Token, TaskCreationOptions.LongRunning,
                    TaskScheduler.Default));
                tasks.Add(Task.Factory.StartNew(() => _writer.Start(), _cancel.Token,
                    TaskCreationOptions.LongRunning, TaskScheduler.Default));
                tasks.Add(Task.Factory.StartNew(() => _reader.Start(), _cancel.Token,
                    TaskCreationOptions.LongRunning, TaskScheduler.Default));

                Console.WriteLine("Hit enter to cancel");
                Console.ReadLine();

                Console.WriteLine("Cancelling...");
                _cancel.Cancel();

                try
                {
                    Task.WaitAll(tasks.ToArray());
                }
                catch (AggregateException e)
                {
                    Console.WriteLine("\nAggregateException thrown with the following inner exceptions:");
                    // Display information about each exception.  
                    foreach (var v in e.InnerExceptions)
                    {
                        if (v is TaskCanceledException)
                            Console.WriteLine("   TaskCanceledException: Task {0}", ((TaskCanceledException) v).Task.Id);
                        else
                            Console.WriteLine("   Exception: {0}", v.GetType().Name);
                    }
                    Console.WriteLine();
                }
                finally
                {
                    _cancel.Dispose();
                }
            }
        }
    }
}