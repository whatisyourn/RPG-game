using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// 定义一个ItemObjectTrigger类，继承自MonoBehaviour
public class ItemObjectTrigger : MonoBehaviour
{
    // 定义一个只读属性myItemObject，用于获取父对象中的ItemObject组件
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();

    // 当其他碰撞体进入触发器时调用此方法
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞体是否为玩家
        if (collision.GetComponent<Player>() != null)
        {
            // 如果玩家已死亡，则返回，不执行后续代码
            if (collision.GetComponent<CharacterStats>().isDead)
                return;
            // 调用myItemObject的PickUpItem方法，拾取物品
            myItemObject.PickUpItem();
        }
    }
}
