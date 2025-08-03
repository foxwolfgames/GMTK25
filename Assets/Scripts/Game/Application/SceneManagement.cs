using System;
using System.Collections;
using System.Collections.Generic;
using FWGameLib.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chronomance.Game
{
    public class SceneManagement : Singleton<SceneManagement>
    {
        [SerializeField] public Scenes[] scenesToLoadForGameplay;
        [SerializeField] public float artificialLoadingDelaySeconds;

        private readonly List<AsyncOperation> _loadOperations = new();
        private float _totalProgress;
        public float TotalLoadProgress => _totalProgress;

        public void ApplicationLoad()
        {
            SceneManager.LoadSceneAsync((int)Scenes.MainMenu, LoadSceneMode.Additive);
        }

        public void LoadGame()
        {
            _loadOperations.Add(SceneManager.UnloadSceneAsync((int)Scenes.MainMenu));
            foreach (Scenes scene in scenesToLoadForGameplay)
            {
                _loadOperations.Add(SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive));
            }

            StartCoroutine(GetSceneLoadProgress(OnLoadingCompletion));
        }

        private void OnLoadingCompletion()
        {
            new GameScenesLoadedEvent().Invoke();
        }

        private IEnumerator GetSceneLoadProgress(Action completionCallback)
        {
            for (int i = 0; i < _loadOperations.Count; i++)
            {
                while (!_loadOperations[i].isDone)
                {
                    foreach (AsyncOperation operation in _loadOperations)
                    {
                        _totalProgress += operation.progress;
                    }

                    _totalProgress /= _loadOperations.Count;
                    yield return null;
                }
            }

            if (artificialLoadingDelaySeconds > 0f)
            {
                yield return new WaitForSeconds(artificialLoadingDelaySeconds);
            }

            completionCallback?.Invoke();
        }
    }
}