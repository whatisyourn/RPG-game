using TMPro; // 引入TextMeshPro命名空间，用于处理文本
using UnityEngine; // 引入Unity引擎命名空间
using UnityEngine.UI; // 引入Unity UI命名空间，用于处理UI元素

// UI_CraftWindow类，继承自MonoBehaviour，用于管理制作窗口的UI
public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName; // 用于显示物品名称的TextMeshProUGUI组件
    [SerializeField] private TextMeshProUGUI itemDescription; // 用于显示物品描述的TextMeshProUGUI组件
    [SerializeField] private Image itemIcon; // 用于显示物品图标的Image组件
    [SerializeField] private Button craftButton; // 制作按钮的Button组件

    [SerializeField] private Image[] materialImage; // 用于显示材料图标的Image数组

    // SetupCraftWindow方法用于设置制作窗口的UI
    public void SetupCraftWindow(ItemData_Equipment _data)
    {
        craftButton.onClick.RemoveAllListeners(); // 移除制作按钮的所有监听器

        // 清除所有材料图标的颜色和文本
        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear; // 将材料图标的颜色设置为透明
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear; // 将材料文本的颜色设置为透明
        }

        // 设置每个材料的图标和数量
        for (int i = 0; i < _data.craftMaterials.Count; i++)
        {
            if (_data.craftMaterials.Count > materialImage.Length) // 检查材料数量是否超过图标数组长度
            {
                Debug.Log("Too much material"); // 输出调试信息
            }
            materialImage[i].sprite = _data.craftMaterials[i].data.icon; // 设置材料图标
            materialImage[i].color = Color.white; // 将材料图标的颜色设置为白色

            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>(); // 获取材料文本组件

            materialSlotText.color = Color.white; // 将材料文本的颜色设置为白色
            materialSlotText.text = _data.craftMaterials[i].stackSize.ToString(); // 设置材料数量文本
        }

        itemIcon.sprite = _data.icon; // 设置物品图标
        itemName.text = _data.itemName; // 设置物品名称
        itemDescription.text = _data.GetDescription(); // 设置物品描述

        // 为制作按钮添加点击事件监听器，调用Inventory的CanCraft方法
        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_data, _data.craftMaterials));
    }
}
