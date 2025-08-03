using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class SpellManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField, Tooltip("How much mana to start with per round")] private int defaultManaStat = 100;
    [SerializeField, Tooltip("Current mana stats")] private int currentMana;

    private PlayerInput playerInput;
    private InputAction cast1Action;

    [System.Serializable]
    public class SpellSlot
    {
        public Spell spell;
        public float lastCastTime = -Mathf.Infinity;
    }
    [SerializeField] private List<SpellSlot> spellList;

    [Header("UI Elements")]
    public ManaIndicator manaIndicator;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        SetCurrentMana(defaultManaStat);
        cast1Action = playerInput.actions["Cast1"];
        cast1Action.performed += context => TryCast(0);
    }
    private void OnEnable()
    {
        RoundStartEvent.Handler += OnRoundReset;
    }

    private void OnDisable()
    {
        RoundStartEvent.Handler -= OnRoundReset;
    }
    private void SetCurrentMana(int mana)
    {
        currentMana = mana;
        manaIndicator.FillAmount = (float)currentMana / (float)defaultManaStat;
        //Debug.Log("Fill amount ratio:" + manaIndicator.fillAmount);
        new ManaValueUpdateEvent(mana).Invoke();
    }
    private void OnRoundReset(RoundStartEvent _)
    {
        SetCurrentMana(defaultManaStat);
    }
    public void TryCast(int index)
    {
        if (index < 0 || index >= spellList.Count) return;

        if (currentMana <= 0)
        {
            Debug.Log("Not enough mana!");
            return;
        }
        Debug.Log($"Spending {spellList[index].spell.manaCost} mana");
        SetCurrentMana(currentMana - spellList[index].spell.manaCost);

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
