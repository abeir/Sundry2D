using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sundry.Event;
using Sundry.GUI;
using UnityEngine;
using UnityEngine.UI;

namespace Sundry.Scene
{
    
    /// <summary>
    /// 用于处理加载场景时的进度条
    /// </summary>
    public class LoadingSceneProgress : MonoBehaviour
    {
        
        [SerializeField]
        private int loadingFadeID = SceneLoadHelper.LoadingSceneFadeID;
        [SerializeField]
        private Image imageProgress;    // 进度条，需要手动设置
        [SerializeField]
        private float durationPerChange = 1f;   // 每次进度改变时的动画时长

        [SerializeField]
        private float delayChangeProgress = 0.5f;       // 延迟进度条的动画，给进入加载场景的淡出动画预留时间
        [SerializeField]
        private float delayChangeScene = 1f;    // 延迟退出当前场景的时间，给退出当前场景时的淡入动画预留时间

        private float _loadingProgress;

        private void Awake()
        {
            imageProgress.fillAmount = 0;
        }

        private void OnEnable()
        {
            _loadingProgress = 0f;
        }

        private IEnumerator Start()
        {
            if (delayChangeProgress > 0)
            {
                yield return new WaitForSeconds(delayChangeProgress);
            }
            
            var tweener = DOTween.To(() => imageProgress.fillAmount, v => imageProgress.fillAmount = v,  _loadingProgress, durationPerChange);
            
            yield return LoadingProgressCoroutine(tweener);

            if (delayChangeScene > 0)
            {
                yield return new WaitForSeconds(delayChangeScene);
            }
            EventManager.TriggerEvent(new StartFadeEvent(loadingFadeID));
        }



        private IEnumerator LoadingProgressCoroutine(TweenerCore< float, float, FloatOptions> tweener)
        {
            while (_loadingProgress < 0.9001f)
            {
                _loadingProgress = SceneLoadHelper.SceneLoader.LoadingProgress;
                // 场景加载进度为 0.9 实际是已经完成了加载，故此处判断大于 0.8999 时将进度设置为 1.0
                if (_loadingProgress > 0.8999)
                {
                    _loadingProgress = 1.0f;
                }

                tweener.ChangeValues(imageProgress.fillAmount, _loadingProgress, durationPerChange);

                Debug.Log(imageProgress.fillAmount);
            
                yield return null;
            }
        }
    }
}