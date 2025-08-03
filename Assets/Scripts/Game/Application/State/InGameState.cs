using FWGameLib.Common.StateMachine;

namespace Chronomance.Game
{
    public class InGameState : IState
    {
        private bool _hasUserInitiatedGameStop;
        public bool HasUserInitiatedGameStop() => _hasUserInitiatedGameStop;

        public void OnEnter()
        {
            InitiateGameStopEvent.Handler += OnInitiateGameStop;
        }

        public void OnExit()
        {
            InitiateGameStopEvent.Handler -= OnInitiateGameStop;
            _hasUserInitiatedGameStop = false;
        }

        private void OnInitiateGameStop(InitiateGameStopEvent _)
        {
            _hasUserInitiatedGameStop = true;
        }
    }
}