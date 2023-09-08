using UnityEngine;

namespace Sundry.Pool
{
    public static class PoolManager
    {


        public static PrefabPool CreatePrefabPool(GameObject origin, Transform parent, bool worldPositionStays = false)
        {
            var go = new GameObject(origin.name + "_pool");
            go.SetActive(false);
            var pool = go.AddComponent<PrefabPool>();
            pool.SetPrefab(origin, parent, worldPositionStays);
            return pool;
        }
        
        public static PrefabPool CreatePrefabPool(GameObject origin, Vector3 position, Quaternion quaternion)
        {
            var go = new GameObject(origin.name + "_pool");
            go.SetActive(false);
            var pool = go.AddComponent<PrefabPool>();
            pool.SetPrefab(origin, position, quaternion);
            return pool;
        }

        public static AudioSourcePool CreateAudioSourcePool(Transform parent, int defaultCapacity, int maxSize)
        {
            return new AudioSourcePool(parent, defaultCapacity, maxSize);
        }
    }
}