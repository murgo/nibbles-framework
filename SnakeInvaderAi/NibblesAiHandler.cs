using System;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Threading;

namespace SnakeInvaderAi
{
    public class NibblesAiHandler
    {
        private readonly INibblesAi _ai;
        private readonly BlockingCollection<dynamic> _incoming;
        private readonly BlockingCollection<dynamic> _outgoing;
        private CancellationToken _token;
        private GameState _gameState;

        public NibblesAiHandler(INibblesAi ai, BlockingCollection<dynamic> incoming, BlockingCollection<dynamic> outgoing, CancellationToken token)
        {
            _ai = ai;
            _incoming = incoming;
            _outgoing = outgoing;
            _token = token;
            _ai.Init(Joined, Control);
        }

        private void Control(Direction dir)
        {
            if (dir == Direction.None) return;

            dynamic o = new ExpandoObject();
            o.msg = "control";
            o.data = new ExpandoObject();
            o.data.direction = (int) dir;

            Console.WriteLine("-> [Control]: " + o.data);

            _outgoing.Add(o, _token);
        }

        private void Joined(string arg1, int arg2)
        {
            dynamic o = new ExpandoObject();
            o.msg = "join";
            o.data = new ExpandoObject();
            o.data.player = new ExpandoObject();
            o.data.player.name = arg1;

            if (arg2 > 0)
                o.data.gameId = arg2.ToString();

            Console.WriteLine("-> [Join]: " + o.data);

            _outgoing.Add(o, _token);
        }

        public void Go()
        {
            while (!_token.IsCancellationRequested)
            {
                var message = _incoming.Take(_token);
                Handle(message);
            }
        }

        private void Handle(dynamic message)
        {
            var type = (string)message.msg;
            var data = message.data;

            if (type == "join")
            {
                Console.WriteLine("<- [Join]: " + data);
                return;
            }

            if (type == "create")
            {
                Console.WriteLine("<- [Create]: " + data);
                return;
            }

            if (type == "start")
            {
                Console.WriteLine("<- [Start]: " + data);
                _gameState = new GameState(data);
                _ai.Start(_gameState);
                return;
            }

            if (type == "end")
            {
                Console.WriteLine("<- [End]: " + data);
                return;
            }

            if (type == "apple")
            {
                Console.WriteLine("<- [Start]: " + data);
                _gameState.UpdateApple(data);
                _ai.Apple(_gameState);
                return;
            }

            if (type == "positions")
            {
                Console.WriteLine("<- [Start]: " + data);
                _gameState.UpdatePositions(data);
                _ai.Positions(_gameState);
                return;
            }
        }
    }
}
