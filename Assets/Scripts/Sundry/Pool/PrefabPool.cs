using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Sundry.Pool
{
    public class PrefabPool : MonoBehaviour, IObjectPool<GameObject>
    {

        private bool _hasParent;
        private GameObject _origin;
        private Transform _parent;
        private bool _worldPositionStays;
        private Vector3 _position;
        private Quaternion _quaternion;

        private readonly List<PrefabItem> _objects = new();
        private int _cursor = -1;
        

        public float lifetime = 1;
        public int preloadAmount = 1;
        public float detectLifetimeInterval = 0.5f;


        public UnityAction<GameObject> CreatedAction;

        private class PrefabItem
        {
            public float ActiveTime;
            public GameObject Instance;
            
            
        }

        private void OnEnable()
        {
            StartCoroutine(nameof(DetectLifetime));
        }

        private void OnDisable()
        {
            StopCoroutine(nameof(DetectLifetime));
        }

        private void OnDestroy()
        {
            Dispose();
        }


        public void Preload()
        {
            for (var i=0; i<preloadAmount; i++)
            {
                Create(false);
            }
        }

        private PrefabItem Create(bool active)
        {
            var obj = Instantiate(_origin);
            obj.SetActive(active);
            ResetObject(obj);
            
            var item = new PrefabItem
            {
                ActiveTime = active ? Time.time : 0,
                Instance = obj
            };
            _objects.Add(item);
            
            CreatedAction?.Invoke(obj);
            return item;
        }

        private IEnumerator DetectLifetime()
        {
            var waitFor = new WaitForSeconds(detectLifetimeInterval);
            while (gameObject.activeSelf)
            {
                yield return waitFor;
                
                foreach (var item in _objects.Where(it => Time.time - it.ActiveTime > lifetime))
                {
                    item.Instance.SetActive(false);
                    ResetObject(item.Instance);
                }
            }
        }


        private void ResetObject(GameObject go)
        {
            if (_hasParent)
            {
                go.transform.SetParent(_parent, _worldPositionStays);
            }
            else
            {
                go.transform.position = _position;
                go.transform.rotation = _quaternion;
            }
        }
        
        public void SetPrefab(GameObject origin, Transform parent, bool worldPositionStays = false)
        {
            _hasParent = true;
            _origin = origin;
            _parent = parent;
            _worldPositionStays = worldPositionStays;
        }

        public void SetPrefab(GameObject origin, Vector3 position, Quaternion quaternion)
        {
            _hasParent = false;
            _origin = origin;
            _position = position;
            _quaternion = quaternion;
        }


        public GameObject Spawn()
        {
            var cursor = NextCursor();
            var item = _objects[cursor];
            if (item.Instance.activeSelf)
            {
                item = Create(true);
                LastCursor();
                return item.Instance;
            }
            item.ActiveTime = Time.time;
            item.Instance.SetActive(true);
            ResetObject(item.Instance);
            return item.Instance;
        }

        public void Despawn(GameObject instance)
        {
            var index = _objects.FindIndex(item => instance == item.Instance);
            if (index < 0)
            {
                return;
            }
            var go = _objects[index].Instance;
            go.SetActive(false);
            ResetObject(go);
        }

        public void Dispose()
        {
            foreach (var obj in _objects)
            {
                Destroy(obj.Instance);
            }
            _objects.Clear();

            foreach (var i in CreatedAction.GetInvocationList())
            {
                CreatedAction -= i as UnityAction<GameObject>;
            }
        }


        private int NextCursor()
        {
            _cursor = (_cursor + 1) % _objects.Count;
            return _cursor;
        }

        private void LastCursor()
        {
            _cursor = _objects.Count - 1;
        }
    }
}