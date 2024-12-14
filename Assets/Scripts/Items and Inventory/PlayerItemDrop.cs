using System.Collections.Generic; // 引入系统集合泛型命名空间
using UnityEngine; // 引入Unity引擎命名空间

// 定义一个PlayerItemDrop类，继承自ItemDrop
public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")] // 在Unity编辑器中显示的标题
    [SerializeField] private float chanceToLoseItems; // 定义一个私有浮点变量，用于存储丢失装备的几率
    [SerializeField] private float chanceToLoseMaterials; // 定义一个私有浮点变量，用于存储丢失材料的几率

    // 重写基类的GenerateDrop方法，用于生成玩家掉落物品
    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance; // 获取当前的库存实例
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>(); // 定义一个列表，用于存储需要卸下的装备
        List<InventoryItem> materialsToLose = new List<InventoryItem>(); // 定义一个列表，用于存储需要丢失的材料

        // 遍历玩家的装备列表，判断是否丢失装备
        foreach (InventoryItem item in inventory.GetEquipmentList()) // 遍历库存中的装备
        {
            // 如果随机数小于等于丢失装备的几率，则将该装备添加到卸下列表中
            if (Random.Range(0, 100) <= chanceToLoseItems)
            {
                DropItem(item.data); // 调用DropItem方法，生成掉落物品
                itemsToUnequip.Add(item); // 将装备添加到卸下列表
            }
        }

        // 遍历需要卸下的装备列表，卸下装备
        foreach (InventoryItem item in itemsToUnequip)
        {
            inventory.UnequipItem(item.data as ItemData_Equipment); // 卸下装备
        }

        // 遍历玩家的材料列表，判断是否丢失材料
        foreach (InventoryItem item in inventory.GetStashList())
        {
            // 如果随机数小于等于丢失材料的几率，则将该材料添加到丢失列表中
            if (Random.Range(0, 100) <= chanceToLoseMaterials)
            {
                DropItem(item.data); // 调用DropItem方法，生成掉落物品
                materialsToLose.Add(item); // 将材料添加到丢失列表
            }
        }

        // 遍历需要丢失的材料列表，移除材料
        foreach (InventoryItem item in materialsToLose)
        {
            inventory.RemoveItem(item.data); // 从库存中移除材料
        }
    }

}
