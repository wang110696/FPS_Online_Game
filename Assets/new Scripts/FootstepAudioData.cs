using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 添加声音的承载脚本
/// </summary>
[CreateAssetMenu(menuName = "FPS/Footstep Audio Data")]
public class FootstepAudioData : ScriptableObject
{
    public List<FootstepAudio> FootstepAudios = new List<FootstepAudio>();
}

/**
 * 一种tag对应的地面可能有多种声音，走路和跑步
 */
[System.Serializable]
public class FootstepAudio
{
    // 加一个踩在不同陆地的材质tag（草地，陆地）
    public string Tag;
    // 加入的音频列表,因为有不同的声音
    public List<AudioClip> AudioClips = new List<AudioClip>();
    // 两只脚之间的间隔
    public float Delay;
}
