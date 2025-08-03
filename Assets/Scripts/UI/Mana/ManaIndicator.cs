using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaIndicator : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text text;
    public Image fill;
    
    [Header("Fill Attributes")]
    public float changeSpeed = 10f;
    public float FillAmount { get; set; } = 1f;

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

    private void Update()
    {
        fill.fillAmount = Mathf.Lerp(fill.fillAmount, FillAmount, changeSpeed * Time.deltaTime);
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