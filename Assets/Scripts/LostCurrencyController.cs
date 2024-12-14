using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// LostCurrencyController类用于处理丢失货币的拾取
public class LostCurrencyController : MonoBehaviour
{
    public int currency; // 货币数量

    // OnTriggerEnter2D方法在其他碰撞体进入触发器时调用
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞体是否是玩家
        if(collision.GetComponent<Player>() != null)
        {
            // 如果是玩家，增加玩家的货币数量
            PlayerManager.instance.currency += currency;
            // 销毁当前游戏对象
            Destroy(this.gameObject);
        }
    }
}
