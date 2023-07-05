using UnityEngine;
using UnityEngine.SceneManagement; //需要使用Unity场景管理API

public class Teleporter : MonoBehaviour
{
    public string targetScene; //目标场景的名字

    //当有其他Collider进入这个区域时会触发这个函数
    void OnTriggerEnter2D(Collider2D other)
    {
        //检查碰撞物是否是车辆（或玩家）
        //这里假设车辆（或玩家）的标签设为"Player"
        if(other.gameObject.tag == "Car")
        {
            //如果是，那么加载目标场景
            SceneManager.LoadScene(targetScene);
        }
    }
}