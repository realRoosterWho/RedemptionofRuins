using UnityEngine;

public class AudioController : MonoBehaviour
{
    // AudioSource数组
    public AudioSource[] audioSources;

    void Start()
    {
        // 为每个声音添加事件监听
        EventManager.OnWallHit += PlayAudio1;
        EventManager.OnWallDestroyed += PlayAudio2;
        // 更多的事件和声音...
    }

    // 播放第一段音频
    public void PlayAudio1()
    {
        if (!audioSources[0].isPlaying)

        {
            audioSources[0].Play();
        }

    }

    // 播放第二段音频
    public void PlayAudio2()
    {
        if (!audioSources[1].isPlaying)
        {
            audioSources[1].Play();
        }
    }

    void OnDestroy()
    {
        // 移除事件监听
        EventManager.OnWallHit -= PlayAudio1;
        EventManager.OnWallDestroyed -= PlayAudio2;
        // 更多的事件和声音...
    }
}