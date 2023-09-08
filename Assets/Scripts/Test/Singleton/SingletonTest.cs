using System;
using Sundry.Singleton;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Test.Singleton
{
    public class SingletonTest : MonoBehaviour
    {
        [Title("Test")]
        [Button]
        public void TestUnitySingleton()
        {
            var obj1 = UnitySingletonImpl.Instance;
            var obj2 = UnitySingletonImpl.Instance;
            
            Debug.Log($"TestUnitySingleton | obj1 == obj2 : {obj1 != null && obj1 == obj2}");
        }

        [Button]
        public void TestUnityPersistentSingleton()
        {
            var obj1 = UnityPersistentSingletonImpl.Instance;
            var obj2 = UnityPersistentSingletonImpl.Instance;
            
            Debug.Log($"TestUnityPersistentSingleton | obj1 == obj2 : {obj1 != null && obj1 == obj2}");
        }
        
    }
    
    
    public class UnitySingletonImpl : UnitySingleton<UnitySingletonImpl>
    {
        private void Start()
        {
            Debug.Log("UnitySingletonImpl Start");
        }
    }

    public class UnityPersistentSingletonImpl : UnityPersistentSingleton<UnityPersistentSingletonImpl>
    {
        private void Start()
        {
            Debug.Log("UnityPersistentSingletonImpl Start");
        }
    }
    
}