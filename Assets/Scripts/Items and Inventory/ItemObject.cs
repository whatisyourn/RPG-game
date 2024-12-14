using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// 定义一个ItemObject类，用于表示游戏中的物品对象
public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb; // 定义一个私有变量rb，用于存储物品的刚体组件
    [SerializeField] private Vector2 velocity; // 定义一个私有变量velocity，用于存储物品的速度
    [SerializeField] private ItemData itemData; // 定义一个私有变量itemData，用于存储物品的数据

    // 设置物品外观的方法
    private void SetupVisuals()
    {
        if (itemData == null) // 如果物品数据为空
        {
            return; // 直接返回，不进行任何操作
        }

        GetComponent<SpriteRenderer>().sprite = itemData.icon; // 获取物品的SpriteRenderer组件，并设置其图标
        gameObject.name = "Item object - " + itemData.itemName; // 设置物品对象的名称
    }

    // 初始化物品的方法，设置物品数据和速度
    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData; // 将传入的物品数据赋值给itemData
        rb.velocity = _velocity; // 设置物品的速度

        SetupVisuals(); // 调用SetupVisuals方法，设置物品外观
    }

    // 拾取物品的方法
    public void PickUpItem()
    {
        // 如果库存已满且物品类型为装备，则不拾取
        if(!Inventory.instance.CanAddItem() && itemData.itemType == ItemTtype.Equipment)
        {
            rb.velocity = new Vector2(0, 7); // 设置物品的速度，使其向上弹起
            PlayerManager.instance.player.fx.CreatePopUpText("Inventory is full"); // 显示提示信息：库存已满
            return; // 直接返回，不进行拾取
        }

        Inventory.instance.AddItem(itemData); // 将物品添加到库存中
        Destroy(gameObject); // 销毁物品对象
    }
}
