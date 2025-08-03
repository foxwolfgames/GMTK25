using FWGameLib.Common.StateMachine;

namespace Chronomance.Game
{
    public class InitialLoadingState : IState
    {
        private bool _hasMainMenuSceneLoaded;
        public bool HasMainMenuSceneLoaded() => _hasMainMenuSceneLoaded;

        public void OnEnter()
        {
            MainMenuSceneLoadedEvent.Handler += OnMainMenuLoaded;
            SceneManagement.Instance.ApplicationLoad();
        }

        public void OnExit()
        {
            MainMenuSceneLoadedEvent.Handler -= OnMainMenuLoaded;
        }

        private void OnMainMenuLoaded(MainMenuSceneLoadedEvent _)
        {
            _hasMainMenuSceneLoaded = true;
        }
    }
}