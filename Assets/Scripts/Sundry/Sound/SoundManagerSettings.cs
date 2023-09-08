using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace Sundry.Sound
{
    
    [Serializable]
    [CreateAssetMenu(menuName = "Sundry/Sound/SoundManagerSettings")]
    public class SoundManagerSettings : ScriptableObject
    {
        public const float MinVolume = -80f;
        public const float MaxVolume = 20f;
        public const float DefaultVolume = 0f;
        
        [Title("Load & Save")]
        public bool autoLoad = true;
        public bool autoSave = true;
        
        [Title("Audio Mixer")]
        public AudioMixer targetMixer;

        public AudioMixerGroup master;
        [LabelText("BGM")]
        public AudioMixerGroup bgm;
        [LabelText("EFF")]
        public AudioMixerGroup eff;
        [LabelText("UI")]
        public AudioMixerGroup ui;
        public AudioMixerGroup other;
        
        [Title("Audio Mixer Exposed Parameters")]
        public string masterVolumeParameter = "MasterVolume";
        public string bgmVolumeParameter = "BGMVolume";
        public string effVolumeParameter = "EFFVolume";
        public string uiVolumeParameter = "UIVolume";
        public string otherVolumeParameter = "OtherVolume";
        

        [BoxGroup("Track", true, true)]
        // [TitleGroup("Track", alignment: TitleAlignments.Centered)]
        [TitleGroup("Track/Master"), ReadOnly, Range(MinVolume, MaxVolume)]
        public float masterVolume = DefaultVolume;
        [TitleGroup("Track/Master"), ReadOnly]
        public bool masterOn = true;
        [TitleGroup("Track/Master"), ReadOnly]
        public float masterVolumeMuted;

        [TitleGroup("Track/BGM"), ReadOnly, Range(MinVolume, MaxVolume)]
        public float bgmVolume = DefaultVolume;
        [TitleGroup("Track/BGM"), ReadOnly]
        public bool bgmOn = true;
        [TitleGroup("Track/BGM"), ReadOnly]
        public float bgmVolumeMuted;

        [TitleGroup("Track/EFF"), ReadOnly, Range(MinVolume, MaxVolume)]
        public float effVolume = DefaultVolume;
        [TitleGroup("Track/EFF"), ReadOnly]
        public bool effOn = true;
        [TitleGroup("Track/EFF"), ReadOnly]
        public float effVolumeMuted;

        [TitleGroup("Track/UI"), ReadOnly, Range(MinVolume, MaxVolume)]
        public float uiVolume = DefaultVolume;
        [TitleGroup("Track/UI"), ReadOnly]
        public bool uiOn = true;
        [TitleGroup("Track/UI"), ReadOnly]
        public float uiVolumeMuted;

        [TitleGroup("Track/Other"), ReadOnly, Range(MinVolume, MaxVolume)]
        public float otherVolume = DefaultVolume;
        [TitleGroup("Track/Other"), ReadOnly]
        public bool otherOn = true;
        [TitleGroup("Track/Other"), ReadOnly]
        public float otherVolumeMuted;
    }
}