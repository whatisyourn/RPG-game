using UnityEngine; // 引入Unity引擎命名空间

// DeadZone类用于处理进入死亡区域的对象
public class DeadZone : MonoBehaviour
{
    // OnTriggerEnter2D方法在其他碰撞体进入触发器时调用
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞体是否具有CharacterStats组件
        if (collision.GetComponent<CharacterStats>() != null)
        {
            // 如果有CharacterStats组件，调用其KillEntity方法
            collision.GetComponent<CharacterStats>().KillEntity();
        }
        else
        {
            // 否则，销毁碰撞体的游戏对象
            Destroy(collision.gameObject); // 销毁一切掉下去的物品
        }
    }
}
