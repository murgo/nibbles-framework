using System.Collections.Generic;

namespace SnakeInvaderAi
{
    public class GameState
    {
        public Point ApplePosition { get; private set; }
        public List<Player> Players { get; private set; }

        public GameState(dynamic data)
        {
            Players = new List<Player>();
            foreach (var player in data.players)
            {
                Players.Add(new Player { Name = player.name });
            }
        }

        public void UpdateApple(dynamic data)
        {
            ApplePosition = new Point { X = data[0], Y = data[1] };
        }

        public void UpdatePositions(dynamic data)
        {
            int index = 0;
            foreach (var snake in data.snakes)
            {
                Players[index].Direction = snake.direction;
                Players[index].Body = new List<Point>();
                foreach (var p in snake.body)
                {
                    Players[index].Body.Add(new Point { X = p[0], Y = p[1]});
                }

                index++;
            }
        }
    }
}