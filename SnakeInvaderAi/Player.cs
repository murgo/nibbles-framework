using System.Collections.Generic;

namespace SnakeInvaderAi
{
    public class Player
    {
        public string Name { get; set; }
        public List<Point> Body { get; set; }
        public Direction Direction { get; set; }
    }
}