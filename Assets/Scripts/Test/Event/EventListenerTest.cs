using Sirenix.OdinInspector;
using Sundry.Event;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Test.Event
{

    public class TestEvent : IEvent
    {
        public int ID => Random.Range(0, int.MaxValue);
    }
    
    public class EventListenerTest : MonoBehaviour, IEventListener<TestEvent>
    {
        [Title("Test")]
        [Button]
        public void TestEventListener()
        {
            var e = new TestEvent();
            
            EventManager.TriggerEvent(e);
        }


        private void OnEnable()
        {
            EventManager.AddListener(this);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(this);
        }

        public void OnTriggered(TestEvent evn)
        {
            Debug.Log($"EventListenerTest.OnTriggered: {evn.ID} | {evn.GetType().Name}");
        }
    }
}