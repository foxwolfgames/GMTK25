using FWGameLib.Common.EditorTools;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private int round;

    public int GetRound() => round;

    private void Start()
    {
        // Set the round manager's scene to be the active scene
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != gameObject.scene.name)
        {
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(gameObject.scene);
        }
    }

    private void OnEnable()
    {
        TriggerResetEvent.Handler += ResetRound;
    }

    private void OnDisable()
    {
        TriggerResetEvent.Handler -= ResetRound;
    }

    // Initiates the restart process of moving the character back to the start
    private void ResetRound(TriggerResetEvent _ = null)
    {
        new RoundStartEvent(round + 1).Invoke();
        round++;
    }

    #region Editor Functions
    [ButtonInvoke("Editor_TriggerRoundReset")] public bool editorResetRound;
    private void Editor_TriggerRoundReset()
    {
        new TriggerResetEvent().Invoke();
    }
    #endregion
}