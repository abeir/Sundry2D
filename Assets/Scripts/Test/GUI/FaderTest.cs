using Sirenix.OdinInspector;
using Sundry.Event;
using Sundry.GUI;
using UnityEngine;

namespace Test.GUI
{
    
    public class FaderTest : MonoBehaviour
    {
        
        [SerializeField]
        private int fadeID;
        
        [Title("Test")]
        [Button]
        public void TestUnityStartFade()
        {
            EventManager.TriggerEvent(new StartFadeEvent(fadeID));

        }
        
    }
}