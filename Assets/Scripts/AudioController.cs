using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{
    // AudioSource数组
    public AudioSource[] audioSources;

    void Start()
    {
        // 为每个声音添加事件监听
        EventManager.OnWallHit += PlayAudio1;
        EventManager.OnWallDestroyed += PlayAudio2;
        EventManager.OnPlayerEnterCar += PlayAudio3;
        EventManager.OnPlayerExitCar += PlayAudio4;
        EventManager.OnMonsterSpawned += PlayAudio5;
        EventManager.OnPullIn += PlayAudio6;
        EventManager.OnPullOut += PlayAudio7;
        EventManager.OnError += PlayAudio8;
        EventManager.OnPlayerGetBucket += PlayAudio9;
        EventManager.OnPlayerDropBucket += PlayAudio10;
        EventManager.OnPlayerNearGasStation += PlayAudio11;
        EventManager.OnPlayerAddingGas += PlayAudio12;
        EventManager.OnPlayerStopAddingGas += PlayAudio13;
        EventManager.OnPlayerAttacked += PlayAudio14;
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
    
    // 播放第三段音频
    public void PlayAudio3()
    {
        // 如果第二段音频不为空，那么停止播放；如果为空，那么不做任何事情
        if (audioSources[2] != null)
        {
            // 延时半秒钟后再播放
            StartCoroutine(PlayDelayedAudio(audioSources[2], 0.5f));
        }

        // 播放audio[5]
        if (audioSources[5] != null)
        {
            audioSources[5].Play();
        }
    }

    private IEnumerator PlayDelayedAudio(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Play();
    }
    
    // 停止播放第三段音频
    public void PlayAudio4()
    {
        //如果音频为空，那么不做任何事情
        if (audioSources[2] != null)
        {
            //如果音频不在播放，那么播放；如果在播放，什么都不做
            if (audioSources[2].isPlaying)
            {
                audioSources[2].Stop();
            }
        }
        
        //Play Audio4
        if (audioSources[4] != null)
        {
            audioSources[4].Play();
        }

    }
    
    public void PlayAudio5(float distance)
    {
        //判断index有没有越界，如果越界，那么不做任何事情
        if (audioSources.Length < 4)
        {
            return;
        }
        
        //如果这段音频为空，那么不做任何事情
        if (audioSources[3] == null)
        {
            return;
        }
        //设置音量大小，距离越近音量越大；分档执行
        if (distance < 2.5)
        {
            audioSources[3].volume = 1;
        }
        else if (distance < 5)
        {
            audioSources[3].volume = 0.8f;
        }
        else if (distance < 7.5)
        {
            audioSources[3].volume = 0.6f;
        }
        else if (distance < 10)
        {
            audioSources[3].volume = 0.4f;
        }
        else if (distance < 12.5)
        {
            audioSources[3].volume = 0.2f;
        }
        else
        {
            audioSources[3].volume = 0;
        }
        //如果第二段音频不为空，那么停止播放；如果为空，那么不做任何事情
        if (audioSources[3] != null && !audioSources[3].isPlaying)
        {
            audioSources[3].Play();
        }
        //如果audioSource[2]在播，那我就不播了
        if (audioSources[2] != null && audioSources[2].isPlaying)
        {
            audioSources[3].Stop();
        }

    }
    
    public void PlayAudio6()
    {
    //如果第二段音频不为空，那么停止播放；如果为空，那么不做任何事情
        if (audioSources[6] != null && !audioSources[6].isPlaying)
        {
            audioSources[6].Play();
        }

    }
    
    public void PlayAudio7()
    {
        //如果第二段音频不为空，那么停止播放；如果为空，那么不做任何事情
        if (audioSources[7] != null && !audioSources[7].isPlaying)
        {
            StartCoroutine(PlayDelayedAudio(audioSources[7], 0.5f));
        }

    }
    
    public void PlayAudio8()
    {
        //如果第二段音频不为空，那么停止播放；如果为空，那么不做任何事情
        if (audioSources[8] != null && !audioSources[8].isPlaying)
        {
            audioSources[8].Play();
        }

    }
    
    public void PlayAudio9()
    {
        //如果第二段音频不为空，那么停止播放；如果为空，那么不做任何事情
        if (audioSources[11] != null && !audioSources[11].isPlaying)
        {
            audioSources[11].Play();
        }

    }
    
    public void PlayAudio10()
    {
        //如果第二段音频不为空，那么停止播放；如果为空，那么不做任何事情
        if (audioSources[12] != null && !audioSources[12].isPlaying)
        {
            audioSources[12].Play();
        }

    }
    
    public void PlayAudio11()
    {
        //如果第二段音频不为空，那么停止播放；如果为空，那么不做任何事情
        if (audioSources[9] != null && !audioSources[9].isPlaying)
        {
            audioSources[9].Play();
        }

    }
    
    public void PlayAudio12()
    {
        //如果第二段音频不为空，那么停止播放；如果为空，那么不做任何事情
        if (audioSources[13] != null && !audioSources[13].isPlaying)
        {
            StartCoroutine(PlayDelayedAudio(audioSources[13], 0.5f));
        }

    }
    
    public void PlayAudio13()
    {
        //如果第二段音频不为空，那么停止播放；如果为空，那么不做任何事情
        
        //如果音频为空，那么不做任何事情
        if (audioSources[13] != null)
        {
            //如果音频不在播放，那么播放；如果在播放，什么都不做
            if (audioSources[13].isPlaying)
            {
                audioSources[13].Stop();
            }
        }
    }
    
    public void PlayAudio14(float aaa)
    {
        //如果第二段音频不为空，那么停止播放；如果为空，那么不做任何事情
        
        if (audioSources[10] != null && !audioSources[10].isPlaying)
        {
            audioSources[10].Play();
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