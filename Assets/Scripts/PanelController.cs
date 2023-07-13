using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    public GameObject panel;

    void Awake()
    {
        panel.SetActive(false); // 面板初始状态为关闭
    }

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf); // 切换面板的显示状态
    }
    
    //退出游戏
    public void QuitGame()
    {
        Application.Quit();
    }
}
