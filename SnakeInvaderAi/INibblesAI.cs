using System;

namespace SnakeInvaderAi
{
    public interface INibblesAi
    {
        void Init(Action<string, int> joined, Action<Direction> control);
        void Start(GameState gameState);
        void Apple(GameState newGameState);
        void Positions(GameState newGameState);
    }
}
