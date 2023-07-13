using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance; // 静态引用，确保只有一个实例存在
    public AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this; // 将当前对象设置为实例
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 如果存在其他实例，则销毁当前对象
        }

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}