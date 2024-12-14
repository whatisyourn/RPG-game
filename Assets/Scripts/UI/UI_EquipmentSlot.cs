using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间
using UnityEngine.EventSystems; // 引入Unity事件系统命名空间

// 定义UI_EquipmentSlot类，继承自UI_ItemSlot类
public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType; // 定义装备槽类型

    // OnValidate方法在编辑器中更改值时调用，用于更新游戏对象的名称
    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString(); // 设置游戏对象的名称为装备槽类型
    }

    // OnPointerDown方法在指针按下时调用，处理装备槽的点击事件
    public override void OnPointerDown(PointerEventData eventData)
    {
        // 如果item或item.data为空，则直接返回
        if(item == null || item.data == null)
        {
            return;
        }

        // 从装备槽中卸下物品
        Inventory.instance.UnequipItem(item.data as ItemData_Equipment);
        // 将物品添加到背包中
        Inventory.instance.AddItem(item.data as ItemData_Equipment); // 先卸下再添加到背包中，避免重复

        // 隐藏物品提示
        ui.itemToolTip.HideToolTip();
        
        // 清空装备槽
        CleanUpSlot();
    }
}
