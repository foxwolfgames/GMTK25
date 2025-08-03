using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class SpellManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField, Tooltip("How much mana to start with per round")] private int defaultManaStat = 100;
    [SerializeField] private int currentMana;

    [Header("Spell Prefabs (match spellList order)")]
    [SerializeField] private List<Spell> spellPrefabs;

    [SerializeField] private List<SpellSlot> spellList;

    private List<ObjectPool<Spell>> spellPools = new();

    [Header("References")]
    [SerializeField] private Transform spellSpawnPoint;

    private PlayerCharacter player;
    private PlayerInput playerInput;
    private InputAction cast1Action;
    private InputAction cast2Action;

    [System.Serializable]
    public class SpellSlot
    {
        public SpellData spell;
        public float lastCastTime = -Mathf.Infinity;
    }

    private void Awake()
    {
        player = GetComponent<PlayerCharacter>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        SetCurrentMana(defaultManaStat);
        cast1Action = playerInput.actions["Cast1"];
        cast1Action.performed += context => TryCast(0);
        cast2Action = playerInput.actions["Cast2"];
        cast2Action.performed += context => TryCast(1);

        for (int i = 0; i < spellPrefabs.Count; i++)
        {
            int poolIndex = i;

            ObjectPool<Spell> pool = null;

            pool = new ObjectPool<Spell>(
                createFunc: () =>
                {
                    Spell spell = Instantiate(spellPrefabs[poolIndex], spellSpawnPoint.position, Quaternion.identity);
                    spell.SetPool(pool);
                    spell.transform.SetParent(SpellPoolManager.Instance.transform);
                    return spell;
                },
                actionOnGet: spell =>
                {
                    spell.gameObject.SetActive(true);
                    spell.Initialize(spellList[poolIndex].spell, gameObject);
                },
                actionOnRelease: spell => spell.gameObject.SetActive(false),
                actionOnDestroy: spell =>
                {
                    if (spell != null)
                        Destroy(spell.gameObject);
                },
                collectionCheck: false,
                defaultCapacity: 5,
                maxSize: 20
            );

            spellPools.Add(pool);
        }

    }

    private void OnEnable()
    {
        RoundStartEvent.Handler += OnRoundReset;
    }

    private void OnDisable()
    {
        RoundStartEvent.Handler -= OnRoundReset;
    }

    private void OnDestroy()
    {
        spellPools.Clear();
    }

    private void SetCurrentMana(int mana)
    {
        currentMana = mana;
        new ManaValueUpdateEvent(mana).Invoke();
    }

    private void OnRoundReset(RoundStartEvent _)
    {
        SetCurrentMana(defaultManaStat);
    }

    public void TryCast(int index)
    {
        Debug.Log($"Try Cast {index}");
        if (index < 0 || index >= spellList.Count) return;

        SpellSlot currentSlot = spellList[index];
        SpellData spellData = currentSlot.spell;

        if (currentMana < spellData.manaCost)
        {
            Debug.Log("Not enough mana!");
            return;
        }

        if (Time.time < currentSlot.lastCastTime + spellData.cooldown)
        {
            Debug.Log($"{spellData.spellName} is on cooldown");
            return;
        }

        currentSlot.lastCastTime = Time.time;
        SetCurrentMana(currentMana - spellData.manaCost);

        spellPools[index].Get();
    }
}
