using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Sundry.Pool
{
    public class AudioSourcePool : IObjectPool<AudioSource>
    {

        private readonly UnityEngine.Pool.IObjectPool<AudioSource> _pool;
        private readonly Transform _parent;

        
        public AudioSourcePool(Transform parent, int defaultCapacity, int maxSize)
        {
            _pool = new ObjectPool<AudioSource>(CreateItem, OnGetItem, OnReleaseItem, null, true, defaultCapacity, maxSize);
            _parent = parent;
        }


        public AudioSource Spawn()
        {
            return _pool.Get();
            
        }

        public void Despawn(AudioSource instance)
        {
            _pool.Release(instance);
        }
        
        public void Dispose()
        {
            _pool.Clear();
        }

        private AudioSource CreateItem()
        {
            var go = new GameObject("AudioSource_auto");
            go.transform.SetParent(_parent);
            go.SetActive(false);

            return go.AddComponent<AudioSource>();
        }

        private void OnGetItem(AudioSource audioSource)
        {
            audioSource.gameObject.SetActive(true);
        }

        private void OnReleaseItem(AudioSource audioSource)
        {
            audioSource.Stop();
            audioSource.gameObject.SetActive(false);
        }

    }
}