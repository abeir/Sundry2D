using Sirenix.OdinInspector;
using Sundry.Event;
using Sundry.Scene;
using UnityEngine;

namespace Test.Scene
{
    public class SceneLoaderTest : MonoBehaviour
    {

        [SerializeField]
        private string nextScene;
        
        [Title("Test")]
        [Button]
        public void TestLoadAsync()
        {
            EventManager.TriggerEvent(new LoadSceneEvent(nextScene));
        }
    }
}