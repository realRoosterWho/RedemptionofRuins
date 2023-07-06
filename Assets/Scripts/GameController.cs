using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //需要使用Unity场景管理API

public class GameController : MonoBehaviour
{
    public float countdownTime = 10f;
    public string GameOverScene = "GameOverScene";

    private void Start()
    {
        StartCoroutine(Countdown());
        //订阅游戏结束事件
        EventManager.OnGameOver += onGameOver;
    }

    private IEnumerator Countdown()
    {
        while (countdownTime > 0)
        {
            yield return new WaitForSeconds(1f); // 每一秒钟减少一次
            countdownTime--; // 倒计时减少一秒
        }
        
        // 倒计时结束，触发游戏结束事件
        EventManager.TriggerEventGameOver();
    }
    
    void onGameOver()
    {
        SceneTracker.Instance.TrackScene();
        // 游戏结束，显示游戏结束的UI
        Debug.Log("Game Over");
        SceneManager.LoadScene(GameOverScene);
    }

    //取消订阅
    void OnDestroy()
    {
        EventManager.OnGameOver -= onGameOver; //解释：当游戏结束时，取消订阅游戏结束事件
    }
}
