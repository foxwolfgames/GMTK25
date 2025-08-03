using UnityEngine;
using System.Collections.Generic;

public class SpellPoolManager : MonoBehaviour
{
    public static SpellPoolManager Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
