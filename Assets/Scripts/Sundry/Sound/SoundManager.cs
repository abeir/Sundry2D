using Sirenix.OdinInspector;
using Sundry.Pool;
using Sundry.Singleton;

namespace Sundry.Sound
{
    
    public enum TrackType { Master, BGM, EFF, UI,  Other}
    
    public class SoundManager : UnityPersistentSingleton<SoundManager>
    {
        
        
        [Title("Settings")]
        public SoundManagerSettings settings;

        [Title("Audio Source Pool")]
        public int poolInitialSize = 5;
        public int poolMaxSize = 10;


        private AudioSourcePool _audioSourcePool;
        
        protected override void Awake()
        {
            base.Awake();

            _audioSourcePool = PoolManager.CreateAudioSourcePool(transform, poolInitialSize, poolMaxSize);
        }

        private void OnEnable()
        {
            for (var i=0; i<poolInitialSize; i++)
            {
                var a = _audioSourcePool.Spawn();
                _audioSourcePool.Despawn(a);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _audioSourcePool.Dispose();
        }
    }
}