using Chronomance.UI;
using FWGameLib.Common.StateMachine;
using UnityEngine;

namespace Chronomance.Game
{
    public class GameApplication : MonoBehaviour
    {
        private readonly FWStateMachine _state = new();
        
        private void Awake()
        {
            InitialLoadingState entrypoint = new();
            MainMenuState mainMenuState = new();
            GameLoadingState gameLoadingState = new();
            InGameState inGameState = new();
            
            _state.AddTransition(entrypoint, mainMenuState, entrypoint.HasMainMenuSceneLoaded);
            _state.AddTransition(mainMenuState, gameLoadingState, mainMenuState.HasUserInitiatedGameStart);
            _state.AddTransition(gameLoadingState, inGameState, gameLoadingState.HasGameLoaded);
            _state.AddTransition(inGameState, mainMenuState, inGameState.HasUserInitiatedGameStop);
            _state.SetState(entrypoint);
        }

        private void Update()
        {
            _state.Tick(Time.deltaTime);
        }

        private void OnEnable()
        {
            UIButtonPressEvent.Handler += OnUIButtonPress;
        }

        private void OnDisable()
        {
            UIButtonPressEvent.Handler -= OnUIButtonPress;
        }

        private void OnUIButtonPress(UIButtonPressEvent e)
        {
            if (e.Action == UIButton.UIButtonAction.Play) SceneManagement.Instance.LoadGame();
        }
    }
}