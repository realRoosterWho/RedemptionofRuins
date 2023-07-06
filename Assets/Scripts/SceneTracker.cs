using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTracker : MonoBehaviour
{
    public static SceneTracker Instance { get; private set; }

    public string PreviousScene { get; private set; }

    private void Awake()
    {
        // 确保只有一个 SceneTracker 实例
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 在场景加载前调用
    public void TrackScene()
    {
        PreviousScene = SceneManager.GetActiveScene().name;
    }
}