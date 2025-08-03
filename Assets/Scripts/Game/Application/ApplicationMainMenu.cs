using UnityEngine;

namespace Chronomance.Game
{
    public class ApplicationMainMenu : MonoBehaviour
    {
        private void Start()
        {
            // Indicate to the application that the main menu has loaded
            new MainMenuSceneLoadedEvent().Invoke();
        }
    }
}