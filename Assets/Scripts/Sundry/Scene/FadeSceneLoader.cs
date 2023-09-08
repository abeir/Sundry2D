using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using Sundry.Event;
using Sundry.GUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Sundry.Scene
{
    
    /// <summary>
    /// 加载场景的实现类，此类的无需手动创建 Fader。
    /// 
    /// 加载步骤如下：
    /// 1. 先加载一个加载进度场景
    /// 2. 触发当前场景的 Fader 进行淡入后进入加载进度场景
    /// 3. 开始加载真正需要切换的场景，将加载进度设置到 LoadingProgress 属性中
    /// 4. 切换场景加载完成后，触发加载进度场景中的 Fader 进行淡入后进入真正的场景
    /// 
    /// 注意项：
    /// 1. 当前场景：会自动创建两个 Fader，一个用于进入此场景时的淡出效果（需要设置为自动显示效果），一个用于切换场景时的淡入效果（需要关闭自动显示效果）
    /// 1. 加载进度场景：此场景中需要手动设置两个 Fader，一个用于进入此场景时的淡出效果（需要设置为自动显示效果），一个用于切换场景时的淡入效果（需要关闭自动显示效果）
    /// 2. 切换的场景：此场景中至少需要一个 Fader，用于进入此场景时的淡出效果（需要设置为自动显示效果），若此场景需要切换其他场景，仍然需要一个用于切换场景时的淡入效果的 Fader（需要关闭自动显示效果）
    /// </summary>
    public class FadeSceneLoader : ASceneLoader<FadeSceneLoader>
    {

        [SerializeField]
        private Canvas changeScenePrefab;
        [SerializeField]
        private string nextScene;       // 下一个切换的场景名
        [SerializeField]
        private string loadingScene;        // 加载进度场景名
        [SerializeField]
        private int loadingFadeID = SceneLoadHelper.LoadingSceneFadeID ;     // 加载进度场景中的淡入 Fader ID

        [Title("Fade Out")]
        [SerializeField]
        private Sprite fadeOutImage;    // 进入当前场景的淡出图片
        [SerializeField]
        private Color fadeOutColor = Color.black;     // 进入当前场景的淡出颜色
        [SerializeField]
        private FadeConfig fadeOutConfig;   // 进入当前场景的淡出效果配置
        
        [Title("Fade In")]
        [SerializeField]
        private Sprite fadeInImage;     // 切换场景的淡入图片
        [SerializeField]
        private Color fadeInColor = Color.black;    // 切换场景的淡入颜色
        [SerializeField]
        private FadeConfig fadeInConfig;        // 切换场景的淡入效果配置

        
        
        #region 内部变量

        private const string ChangeSceneCanvasName = "ChangeSceneCanvas_auto";      // 创建 Prefab 实例的对象名
        
        private AsyncOperation _nextOpt;

        #endregion


        protected override void Awake()
        {
            base.Awake();
            
            if (!IsExistsCanvas())
            {
                CreateFades();
            }
        }


        public override void Load()
        {
            if (string.IsNullOrEmpty(nextScene))
            {
                Debug.LogError("Not setup nextScene");
                return;
            }

            EventManager.TriggerEvent(new StartFadeEvent(fadeInConfig.fadeID));
        }
        
        
        public override void OnTriggered(LoadSceneEvent evn)
        {
            if (string.IsNullOrEmpty(evn.SceneName))
            {
                return;
            }

            nextScene = evn.SceneName;
            Load();
        }

        public override void OnTriggered(FadeFinishEvent evn)
        {
            // 当前场景的淡入完成，开始加载下个场景
            if (evn.ID == fadeInConfig.fadeID)
            {
                StartCoroutine(LoadSceneCoroutine());
            }
            
            if (evn.ID == loadingFadeID)
            {
                _nextOpt.allowSceneActivation = true;
            }
        }

        
        private IEnumerator LoadSceneCoroutine()
        {
            yield return CreateLoadingSceneCoroutine();
            yield return CreateSceneCoroutine(nextScene);
        }
        

        private IEnumerator CreateLoadingSceneCoroutine()
        {
            var opt = SceneManager.LoadSceneAsync(loadingScene);
            opt.allowSceneActivation = true;
            while (!opt.isDone)
            {
                yield return null;
            }
        }


        private IEnumerator CreateSceneCoroutine(string scene)
        {
            _nextOpt = SceneManager.LoadSceneAsync(scene);
            _nextOpt.allowSceneActivation = false;
            while (!_nextOpt.isDone)
            {
                LoadingProgress = _nextOpt.progress;
                yield return null;
            }

            EventManager.TriggerEvent(new StartFadeEvent(loadingFadeID));
        }

        private bool IsExistsCanvas()
        {
            var canvasArray = FindObjectsOfType<Canvas>();
            if (canvasArray == null || canvasArray.Length == 0)
            {
                return false;
            }

            return canvasArray.Any(c => c.gameObject.name == ChangeSceneCanvasName);
        }


        private void CreateFades()
        {
            var canvas = Instantiate(changeScenePrefab);
            canvas.name = ChangeSceneCanvasName;
            
            var images = canvas.GetComponentsInChildren<Image>();
            foreach (var image in images)
            {
                var fader = image.GetComponent<Fader>();
                if (image.gameObject.name == "FadeOut")
                {
                    if (fadeOutImage != null)
                    {
                        image.sprite = fadeOutImage;
                    }
                    image.color = fadeOutColor;
                    fader.config = fadeOutConfig;
                }
                else if (image.gameObject.name == "FadeIn")
                {
                    if (fadeInImage != null)
                    {
                        image.sprite = fadeInImage;
                    }
                    image.color = fadeInColor;
                    fader.config = fadeInConfig;
                }
            }
            
            
        } 
        
    }
}