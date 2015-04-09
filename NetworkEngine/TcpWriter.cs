using System.IO;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;

namespace NetworkEngine
{
    public class TcpWriter
    {
        private readonly CancellationToken _token;
        private readonly IPipe<dynamic> _pipe;
        private readonly StreamWriter _writer;

        public TcpWriter(NetworkStream stream, CancellationToken token, IPipe<dynamic> pipe)
        {
            _writer = new StreamWriter(stream);
            _token = token;
            _pipe = pipe;
        }

        public void Start()
        {
            while (!_token.IsCancellationRequested)
            {
                dynamic message = _pipe.SendQueue.Take(_token);

                var json = JsonConvert.SerializeObject(message);
                _writer.WriteLine(json);
                _writer.Flush();
            }
        }
    }
}