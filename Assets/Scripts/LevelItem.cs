using System.Collections;
using System.Collections.Generic;
//导入UI
using UnityEngine.UI;
using UnityEngine;
//导入场景管理
using UnityEngine.SceneManagement;

public class LevelItem : MonoBehaviour
{
    private int LevelId; // 关卡ID
    private Button btn; // 创建按钮

    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    public void Init(int id, bool isLock)
    {
        LevelId = id;
        if (isLock)
        {
            btn.interactable = false;
        }
        else
        {
            btn.interactable = true;
        }
    }

    private void OnClick()
    {
        SceneManager.LoadScene(LevelId); // 场景加载，进入关卡
    }
}

