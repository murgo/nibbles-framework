using System;
using System.Linq;

namespace SnakeInvaderAi
{
    public abstract class BaseNibblesAi : INibblesAi
    {
        private readonly string _name = "Jim_" + Guid.NewGuid().ToString().Substring(Guid.NewGuid().ToString().Length - 2);
        private Action<string, int> _joined;
        private Action<Direction> _control;
        private GameState _gameState;

        public int PlayerIndex { get; set; }

        protected GameState GameState
        {
            get { return _gameState; }
            set
            {
                _gameState = value;
                PlayerIndex = _gameState.Players.Select((player, index) => new { player, index }).First(o => o.player.Name == _name).index;
            }
        }

        public void Init(Action<string, int> joined, Action<Direction> control)
        {
            _joined = joined;
            _control = control;
            _joined(_name, 1);
        }

        public void Start(GameState gameState)
        {
            GameState = gameState;
            _control(React(GameState));
        }

        public void Apple(GameState newGameState)
        {
            GameState = newGameState;
            _control(React(GameState));
        }

        public void Positions(GameState newGameState)
        {
            GameState = newGameState;
            _control(React(GameState));
        }

        protected abstract Direction React(GameState gameState);
    }
}