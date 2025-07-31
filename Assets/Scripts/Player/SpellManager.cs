using UnityEngine;
using UnityEngine.InputSystem;

public class SpellManager : MonoBehaviour
{
    [SerializeField] private Spell[] spellList;

    private PlayerInput playerInput;
    private InputAction cast1Action;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        cast1Action = playerInput.actions["Cast1"];
        cast1Action.performed += _ => CastSpell(0);
    }

    private void CastSpell(int index)
    {
        if (index > 0) return;

        if (index < spellList.Length && spellList[index] != null)
        {
            spellList[index].TryCast(gameObject);
        }
    }
}
