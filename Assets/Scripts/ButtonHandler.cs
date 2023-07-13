using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Button[] levelButtons; // 关卡按钮数组

    void Start()
    {
        // 为每个关卡按钮添加点击事件监听器
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1;
            levelButtons[i].onClick.AddListener(() => SelectLevel(levelIndex));
        }
    }
    
    void SelectLevel(int levelIndex)
    {
        // 根据关卡索引加载对应的场景
        SceneManager.LoadScene("Level_" + levelIndex);
    }
}