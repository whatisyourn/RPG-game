using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using TMPro; // 引入TextMeshPro命名空间，用于UI文本处理
using UnityEngine; // 引入Unity引擎命名空间

// UI_ItemToolTip类继承自UI_ToolTip，用于显示物品信息的工具提示
public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemNameText; // 序列化字段，用于显示物品名称的文本组件
    [SerializeField] private TextMeshProUGUI itemTypeText; // 序列化字段，用于显示物品类型的文本组件
    [SerializeField] private TextMeshProUGUI itemDescription; // 序列化字段，用于显示物品描述的文本组件

    [SerializeField] private int defaultFontSize = 32; // 序列化字段，默认字体大小为32

    // ShowToolTip方法用于显示物品信息的工具提示
    public void ShowToolTip(ItemData_Equipment item)
    {
        if(item == null) // 检查传入的物品是否为空
        {
            return; // 如果为空，直接返回
        }

        itemNameText.text = item.itemName; // 设置物品名称文本
        itemTypeText.text = item.equipmentType.ToString(); // 设置物品类型文本
        itemDescription.text = item.GetDescription(); // 设置物品描述文本
        gameObject.SetActive(true); // 激活工具提示对象

        AdjustFontSize(itemNameText); // 调整物品名称文本的字体大小
        AdjustPosition(); // 调整工具提示的位置
    }

    // HideToolTip方法用于隐藏物品信息的工具提示
    public void HideToolTip()
    {
        itemNameText.fontSize = defaultFontSize; // 重置物品名称文本的字体大小为默认值
        gameObject.SetActive(false); // 隐藏工具提示对象
    }
}
