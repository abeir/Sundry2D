using System;
using System.Collections;
using Sirenix.OdinInspector;
using Sundry.Event;
using Sundry.Helper;
using UnityEngine;
using UnityEngine.UI;

namespace Sundry.GUI
{

    /// <summary>
    /// 开始淡入淡出，通过发送该事件来启动淡入淡出效果，其中的 ID 要与待执行的 Fader 组件中的 fadeID 匹配
    /// </summary>
    public class StartFadeEvent : IEvent
    {
        public int ID { get; }
        
        public StartFadeEvent(int id)
        {
            ID = id;
        }
    }

    /// <summary>
    /// Fader 执行完成后会发送此事件，会将 Fader 组件中的 fadeID 设置为此事件的 ID
    /// </summary>
    public class FadeFinishEvent : IEvent
    {
        public int ID { get; }

        public FadeFinishEvent(int id)
        {
            ID = id;
        }
    }


    [Serializable]
    public class FadeConfig
    {
        [SerializeField]
        public int fadeID;     // 在使用StartFadeEvent事件调用此功能时的ID
        [SerializeField]
        public Fader.FaderMode mode = Fader.FaderMode.FadeIn;      // 淡入或淡出
        [SerializeField]
        public float duration = 0.5f;      // 淡入或淡出的持续时间
        [SerializeField]
        public TweenType tweenType;    // 动画

        [SerializeField]
        public bool ignoreTimescale = true;    // 是否忽略时间缩放
        [SerializeField]
        public bool autoFade;  // 是否自动执行淡入或淡出
        [SerializeField, ShowIf("autoFade")]
        public int delayFrame = 2;     // 延迟指定的帧数后执行淡入淡出，仅当 autoFade=true 有效
    }

    
    [RequireComponent(typeof(Image)), RequireComponent(typeof(CanvasGroup))]
    public class Fader : MonoBehaviour, IEventListener<StartFadeEvent>
    {
        /// <summary>
        /// 淡入淡出模式，淡入表示图片的 alpha 由 0 逐渐变为 1，淡出表示图片的 alpha 由 1 逐渐变为 0
        /// </summary>
        public enum FaderMode
        {
            /// <summary>
            /// 淡入，图片的 alpha 由 0 逐渐变为 1
            /// </summary>
            FadeIn,
            /// <summary>
            /// 淡出，图片的 alpha 由 1 逐渐变为 0
            /// </summary>
            FadeOut
        }

        [SerializeField]
        public FadeConfig config;
        

        #region 内部变量

        private Image _image;
        private CanvasGroup _canvasGroup;
        private bool _running;

        #endregion

        private void Awake()
        {
            _image = GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();

            _image.raycastTarget = false;

            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = true;

            // 根据淡入或淡出，设置 alpha 的初始值
            _canvasGroup.alpha = config.mode == FaderMode.FadeIn ? 0 : 1;
        }

        private void OnEnable()
        {
            EventManager.AddListener(this);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(this);
        }

        private void Start()
        {
            if (!config.autoFade)
            {
                return;
            }
            StartCoroutine(WaitFrameStartFadeCoroutine(config.delayFrame));
        }

        public void StartFade()
        {
            if (_running)
            {
                return;
            }
            StartCoroutine(nameof(StartFadeCoroutine));
        }


        private IEnumerator StartFadeCoroutine()
        {
            _running = true;
            _canvasGroup.blocksRaycasts = true;
            
            float time = 0;
            while (time < config.duration)
            {
                var deltaTime = config.ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime;
                time += deltaTime;

                var progress = Maths.Remap(time, 0, config.duration, 0, 1.0f);
                if (config.mode == FaderMode.FadeOut)
                {
                    progress = 1 - progress;
                }
                _canvasGroup.alpha = config.tweenType.DoTween(progress);
                yield return null;
            }
            _canvasGroup.blocksRaycasts = false;
            
            EventManager.TriggerEvent(new FadeFinishEvent(config.fadeID));
            
            _running = false;
        }

        private IEnumerator WaitFrameStartFadeCoroutine(int count)
        {
            if (count > 0)
            {
                yield return new WaitForFrame(count); 
            }
            yield return StartFadeCoroutine();
        }

        public void OnTriggered(StartFadeEvent evn)
        {
            if (evn.ID != config.fadeID)
            {
                return;
            }
            StartFade();
        }
    }
}