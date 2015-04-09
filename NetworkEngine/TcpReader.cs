using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace NetworkEngine
{
    public class TcpReader
    {
        private readonly CancellationToken _token;
        private readonly IPipe<dynamic> _pipe;
        private StreamReader _streamReader;
        private StringBuilder _buffer = new StringBuilder();
        private char _lastChar;

        public TcpReader(NetworkStream stream, CancellationToken token, IPipe<dynamic> pipe)
        {
            _streamReader = new StreamReader(stream);
            _token = token;
            _pipe = pipe;
        }

        public void Start()
        {
            while (!_token.IsCancellationRequested)
            {
                var c = (char)_streamReader.Read();
                _token.ThrowIfCancellationRequested();
                if (c == -1)
                {
                    Thread.Sleep(1);
                    continue;
                }

                if (_lastChar == '}' && c == '{')
                {
                    var s = _buffer.ToString();
                    _buffer.Clear();
                    var o = JsonConvert.DeserializeObject(s);
                    _pipe.Receive(o);
                }

                _lastChar = c;
                _buffer.Append(c);
            }
        }
    }
}