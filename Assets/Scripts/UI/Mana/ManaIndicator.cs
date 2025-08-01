using TMPro;
using UnityEngine;

public class ManaIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void OnEnable()
    {
        // Show as empty text on enable to prevent showing default value
        text.SetText("");
        ManaValueUpdateEvent.Handler += OnManaValueUpdate;
    }

    private void OnDisable()
    {
        ManaValueUpdateEvent.Handler -= OnManaValueUpdate;
    }
    
    private void UpdateValue(int mana)
    {
        text.SetText($"Mana: {mana}");
    }

    private void OnManaValueUpdate(ManaValueUpdateEvent e)
    {
        UpdateValue(e.NewMana);
    }
}