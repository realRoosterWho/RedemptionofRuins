using UnityEngine;
using TMPro; // 注意需要引入TextMeshPro的命名空间
using System.Collections;

public class UIController_Log : MonoBehaviour
{
    public TextMeshProUGUI eventText; // TextMeshPro UI组件引用
    private Coroutine textCoroutine; // 用来跟踪当前是否有一个正在运行的Coroutine

    private void Start()
    {
        EventManager.OnLogTriggered += OnLogTriggered;
    }

    private void OnDestroy()
    {
        EventManager.OnLogTriggered -= OnLogTriggered;
    }

    private void OnLogTriggered(string message)
    {
        eventText.text = message; // 当事件被触发时，更新TextMeshProUGUI的文本

        // 如果已经有一个Coroutine在运行，先停止它
        if (textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
        }

        // 启动新的Coroutine来在2秒后清空文本
        textCoroutine = StartCoroutine(ClearTextAfterDelay(2f));
    }

    private IEnumerator ClearTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 等待指定的时间
        eventText.text = ""; // 清空文本
    }
}