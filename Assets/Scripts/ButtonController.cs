using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonPanelController : MonoBehaviour
{
    public GameObject panel;
    public Button startButton;
    public Button quitButton;

    void Awake()
    {
        panel.SetActive(false); // 面板初始状态为关闭

        // 添加按钮点击事件监听器
        startButton.onClick.AddListener(OpenPanel);
        quitButton.onClick.AddListener(QuitGame);
    }

    void OpenPanel()
    {
        panel.SetActive(true); // 打开按钮面板
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}