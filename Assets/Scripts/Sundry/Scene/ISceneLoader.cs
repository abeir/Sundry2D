using Sundry.Event;
using Sundry.GUI;
using Sundry.Singleton;
using UnityEngine;

namespace Sundry.Scene
{
    
    /// <summary>
    /// 通过发送此事件开始加载场景
    /// </summary>
    public class LoadSceneEvent : IEvent
    {
        public string SceneName { get; }

        public LoadSceneEvent(string sceneName)
        {
            SceneName = sceneName;
        }
    }
    
    public interface ISceneLoader : IEventListener<LoadSceneEvent>, IEventListener<FadeFinishEvent>
    {
        public float LoadingProgress { get; }
        public void Load();
    }

    public abstract class ASceneLoader<T> : UnityPersistentSingleton<T>, ISceneLoader where T : Component
    {
        public float LoadingProgress { get; protected set; }
        
        public abstract void Load();
        
        public abstract void OnTriggered(LoadSceneEvent evn);
        public abstract void OnTriggered(FadeFinishEvent evn);
        
        
        protected virtual void OnEnable()
        {
            SceneLoadHelper.SceneLoader = this;
            
            EventManager.AddListener<LoadSceneEvent>(this);
            EventManager.AddListener<FadeFinishEvent>(this);
        }

        protected virtual void OnDisable()
        {
            EventManager.RemoveListener<LoadSceneEvent>(this);
            EventManager.RemoveListener<FadeFinishEvent>(this);
        }
    }
}