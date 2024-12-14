using UnityEngine.UI; // 引入Unity UI命名空间，用于处理UI元素
using TMPro; // 引入TextMeshPro命名空间，用于处理文本
using UnityEngine; // 引入Unity引擎命名空间
using UnityEngine.EventSystems; // 引入Unity事件系统命名空间，用于处理用户输入事件

// 定义UI_ItemSlot类，继承自MonoBehaviour，并实现IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler接口，用于处理鼠标事件
public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler   
{
    [SerializeField] protected Image itemImage; // 序列化字段，用于在编辑器中设置物品图像
    [SerializeField] protected TextMeshProUGUI itemText; // 序列化字段，用于在编辑器中设置物品文本

    public InventoryItem item; // 当前存放的物品
    protected UI ui; // UI引用，用于访问父UI组件

    // Start方法在对象创建时调用，初始化UI引用
    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>(); // 获取父对象中的UI组件并赋值给ui变量
    }

    // UpdateSlot方法用于更新物品槽的显示
    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem; // 更新当前物品
        itemImage.color = Color.white; // 设置物品图像的颜色为白色
        if (item != null) // 如果物品不为空
        {
            itemImage.sprite = item.data.icon; // 设置物品图像的精灵为物品的图标
            // 如果当前物品的堆叠数量大于1，则显示物品的堆叠数量
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString(); // 显示堆叠数量
            }
            else
            {
                itemText.text = ""; // 否则清空文本
            }
        }
    }

    // CleanUpSlot方法用于清空物品槽
    public void CleanUpSlot()
    {
        item = null; // 清空当前物品

        itemImage.sprite = null; // 清空物品图像的精灵
        itemImage.color = Color.clear; // 设置物品图像的颜色为透明
        itemText.text = ""; // 清空物品文本
    }

    // OnPointerDown方法在鼠标按下时调用，处理物品的点击事件
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(item == null) // 如果物品为空
        {
            return; // 直接返回
        }

        // 如果按下Control键，则删除物品
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data); // 从库存中移除物品
            return; // 返回
        }

        // 如果物品类型为装备
        if(item.data.itemType == ItemTtype.Equipment)
        {
            Inventory.instance.EquipItem(item.data); // 装备物品
        }

        ui.itemToolTip.HideToolTip(); // 装备完毕，关闭信息提示
    }

    // OnPointerEnter方法在鼠标进入时调用，显示物品信息
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item == null) // 如果物品为空
        {
            return; // 直接返回
        }

        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment); // 显示物品信息提示
    }

    // OnPointerExit方法在鼠标移出时调用，隐藏物品信息
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null) // 如果物品为空
        {
            return; // 直接返回
        }
        ui.itemToolTip.HideToolTip(); // 隐藏物品信息提示
    }
}
