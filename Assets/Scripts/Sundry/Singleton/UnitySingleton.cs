﻿using UnityEngine;

namespace Sundry.Singleton
{
    public class UnitySingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                _instance = FindObjectOfType<T>();
                if (_instance != null)
                {
                    return _instance;
                }
                var obj = new GameObject
                {
                    name = typeof(T).Name + "_Auto"
                };
                _instance = obj.AddComponent<T>();
                return _instance;
            }
        }
    }
}