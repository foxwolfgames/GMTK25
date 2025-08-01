using System.Collections.Generic;
using FWGameLib.Common;
using UnityEngine;

namespace Chronomance.Audio
{
    public class AudioSystem : Singleton<AudioSystem>
    {
        [Header("Audio Source Pooling")]
        [SerializeField] private GameObject pooledAudioSourcePrefab;
        [SerializeField] private int audioSourcePoolSize = 30;

        private List<GameObject> _audioSourcePool;

        private void Awake()
        {
            _audioSourcePool = new List<GameObject>();
            for (var i = 0; i < audioSourcePoolSize; i++)
            {
                var pooledAudioSource = Instantiate(
                    pooledAudioSourcePrefab,
                    transform,
                    true
                );
                pooledAudioSource.SetActive(false);
                _audioSourcePool.Add(pooledAudioSource);
            }
        }
    }
}