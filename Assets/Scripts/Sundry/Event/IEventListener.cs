namespace Sundry.Event
{
    public interface IEvent
    {
    }



    public interface IEventListener
    {
    }
    
    public interface IEventListener<in T> : IEventListener where T : IEvent
    {
        public void OnTriggered(T evn);
    }

}