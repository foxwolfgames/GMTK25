using UnityEngine;

namespace FWGameLib.Common
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] containingObjects = FindObjectsByType(typeof(T), FindObjectsSortMode.None) as T[];

                    if (containingObjects.Length != 0)
                    {
                        _instance = containingObjects[0];
                    }

                    if (containingObjects.Length > 1)
                    {
                        Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                    }
                    
                    // Create new object to address singleton not existing
                    if (_instance == null)
                    {
                        _instance = new GameObject
                        {
                            name = $"_{typeof(T).Name}"
                        }.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }
    }
}