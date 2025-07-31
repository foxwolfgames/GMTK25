using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellManager : MonoBehaviour
{
    [System.Serializable]
    public class SpellSlot
    {
        public Spell spell;
        public float lastCastTime = -Mathf.Infinity;
    }
    [SerializeField] private List<SpellSlot> spellList;

    private PlayerInput playerInput;
    private InputAction cast1Action;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        cast1Action = playerInput.actions["Cast1"];
        cast1Action.performed += context => TryCast(0);
    }

    public void TryCast(int index)
    {
        if (index < 0 || index >= spellList.Count) return;

        SpellSlot currentSpell = spellList[index];
        
        if (Time.time >= currentSpell.lastCastTime + currentSpell.spell.cooldown)
        {
            currentSpell.spell.Cast(gameObject);
            currentSpell.lastCastTime = Time.time;
        }
        else
        {
            Debug.Log($"{currentSpell.spell.spellName} is on cooldown");
        }
    }
}
