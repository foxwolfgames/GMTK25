using FWGameLib.Common.StateMachine;

namespace Chronomance.Game
{
    public class MainMenuState : IState
    {
        private bool _hasUserInitiatedGameStart;
        public bool HasUserInitiatedGameStart() => _hasUserInitiatedGameStart;

        public void OnEnter()
        {
            InitiateGameStartEvent.Handler += OnInitiateGameStart;
        }

        public void OnExit()
        {
            InitiateGameStartEvent.Handler -= OnInitiateGameStart;
            _hasUserInitiatedGameStart = false;
        }

        private void OnInitiateGameStart(InitiateGameStartEvent _)
        {
            _hasUserInitiatedGameStart = true;
        }
    }
}