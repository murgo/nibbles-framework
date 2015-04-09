using System.Linq;

namespace SnakeInvaderAi
{
    public class NibblesAi : BaseNibblesAi
    {
        protected override Direction React(GameState gameState)
        {
            var player = gameState.Players[PlayerIndex];
            if (player == null || player.Body == null)
            {
                return Direction.None;
            }

            var headPosition = player.Body.FirstOrDefault();
            if (headPosition == null) return Direction.None;

            if (headPosition.X < gameState.ApplePosition.X)
            {
                return player.Direction == Direction.Left ? Direction.Up : Direction.Right;
            }

            if (headPosition.X > gameState.ApplePosition.X)
            {
                return player.Direction == Direction.Right ? Direction.Up : Direction.Left;
            }

            if (headPosition.Y < gameState.ApplePosition.Y)
            {
                return player.Direction == Direction.Up ? Direction.Left : Direction.Down;
            }

            if (headPosition.Y > gameState.ApplePosition.Y)
            {
                return player.Direction == Direction.Down ? Direction.Right : Direction.Up;
            }

            return Direction.None;
        }
    }
}