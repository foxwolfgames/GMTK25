using FWGameLib.Common.StateMachine;

namespace Chronomance.Game
{
    public class GameLoadingState : IState
    {
        private bool _hasGameLoaded;
        public bool HasGameLoaded() => _hasGameLoaded;

        public void OnEnter()
        {
            GameScenesLoadedEvent.Handler += OnGameScenesLoaded;
            SceneManagement.Instance.LoadGame();
        }

        public void OnExit()
        {
            GameScenesLoadedEvent.Handler -= OnGameScenesLoaded;
            _hasGameLoaded = false;
        }

        private void OnGameScenesLoaded(GameScenesLoadedEvent _)
        {
            _hasGameLoaded = true;
        }
    }
}