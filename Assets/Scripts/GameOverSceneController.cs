using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverSceneController : MonoBehaviour
{
    public Button replayButton;
    public Button mainMenuButton;
    private string previousSceneName;

    void Start()
    {
        // 添加按钮事件
        replayButton.onClick.AddListener(ReplayGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        previousSceneName = null; 
        Debug.Log("Previous Scene Name: " + previousSceneName);
        
        //不按按钮三秒后Replay
        //Invoke("ReplayGame", 3f);
        
    }

    void ReplayGame()
    {
        // 重载当前场景
        SceneManager.LoadScene(SceneTracker.Instance.PreviousScene);

    }

    void GoToMainMenu()
    {
        // 加载主菜单场景
        SceneManager.LoadScene("MainMenuScene");
    }
}