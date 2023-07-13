using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource audioSource;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}