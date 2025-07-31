using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMagic : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField, Tooltip("How much mana to start with per round")] private int defaultManaStat = 100;
    [SerializeField, Tooltip("Current mana stats")] private int currentMana;

    private PlayerInput _input;
    
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        SetCurrentMana(defaultManaStat);
    }

    private void OnEnable()
    {
        RoundStartEvent.Handler += OnRoundReset;
        _input.actions["Attack"].performed += DoMagic;
    }

    private void OnDisable()
    {
        RoundStartEvent.Handler -= OnRoundReset;
        _input.actions["Attack"].performed -= DoMagic;
    }

    private void SetCurrentMana(int mana)
    {
        currentMana = mana;
        new ManaValueUpdateEvent(mana).Invoke();
    }

    private void DoMagic(InputAction.CallbackContext _)
    {
        // TODO: proper magic logic
        if (currentMana < 20)
        {
            Debug.Log("Not enough mana!");
            return;
        }

        Debug.Log("Magic!!!");
        SetCurrentMana(currentMana - 20);
    }

    private void OnRoundReset(RoundStartEvent _)
    {
        SetCurrentMana(defaultManaStat);
    }
}