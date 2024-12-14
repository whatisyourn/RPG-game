// 引入Unity.VisualScripting命名空间
using Unity.VisualScripting;
// 引入UnityEngine.EventSystems命名空间，用于处理事件系统
using UnityEngine.EventSystems;

// 定义UI_CraftSlot类，继承自UI_ItemSlot类
public class UI_CraftSlot : UI_ItemSlot
{
    // 重写Start方法，初始化时调用
    protected override void Start()
    {
        base.Start(); // 调用基类的Start方法
    }

    // SetuoCraftSlot方法用于设置制作槽的UI显示
    public void SetuoCraftSlot(ItemData_Equipment _data)
    {
        // 如果传入的数据为空，则直接返回
        if (_data == null) {
            return;
        }
        // 将传入的数据赋值给item的data属性
        item.data = _data;
        // 设置itemImage的sprite为传入数据的icon
        itemImage.sprite = _data.icon;
        // 设置itemText的文本为传入数据的itemName
        itemText.text = _data.itemName;
        // 如果itemName的长度大于12，则缩小字体
        if(itemText.text.Length > 12)
        {
            itemText.fontSize = itemText.fontSize * .7f; // 将字体大小缩小为原来的70%
        }
        else
        {
            itemText.fontSize = 24; // 否则设置字体大小为24
        }
    }

    // OnPointerDown方法用于处理指针按下事件
    public override void OnPointerDown(PointerEventData eventData)
    {
        // 设置制作窗口的内容为当前item的数据
        ui.craftWindow.SetupCraftWindow(item.data as ItemData_Equipment);
    }
}
