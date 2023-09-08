using System;
using UnityEngine;

namespace Sundry.Sound
{
    [Serializable]
    public struct PlayOperations
    {
        
        public TrackType trackType;

        public Vector3 location;    // 声音播放的位置

        public bool loop;

        public float volume;
        
        
        public float playbackTime;  // 开始播放的时间

        public float playbackDuration;  // 播放的时长

        public float panStereo;     // 立体声声向
        
        public float spatialBlend;      // 设置此 AudioSource 受3D空间化计算（衰减、多普勒等）影响的程度。0.0使声音完全为2D，1.0使其完全为3D


        public bool soloSingleTrack;        // 只播放当前的声音，当前轨道其他声音静音
        public bool soloAllTrack;       // 只播放当前的声音，所有轨道都静音
        public bool autoUnSoloOnEnd;        // solo结束时，被静音的轨道恢复播放

        public bool bypassEffects;     // 旁通效果器
        public bool bypassListenerEffects;
        public bool bypassReverbZones;

    }
}