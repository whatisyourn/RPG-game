using System.Collections.Generic; // 引入系统集合命名空间
using UnityEngine; // 引入Unity引擎命名空间
using UnityEngine.EventSystems; // 引入Unity事件系统命名空间

// UI_CraftList类，继承自MonoBehaviour，实现IPointerDownHandler接口，用于管理制作列表的UI
public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent; // 制作槽的父级Transform
    [SerializeField] private GameObject craftSlotPrefab; // 制作槽的预制体

    [SerializeField] private List<ItemData_Equipment> craftEquipment; // 可制作装备的列表

    // Start方法在对象创建时调用，初始化制作列表和默认制作窗口
    void Start()
    {
        // 调用父级的第一个子对象的UI_CraftList组件的SetupCraftList方法
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
        // 初始化默认的制作窗口
        SetupDefaultCraftWindow();
    }

    // SetupCraftList方法用于设置制作列表
    public void SetupCraftList()
    {
        // 清空当前所有的制作槽
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject); // 销毁每个子对象
        }
        // 为每个可制作装备创建一个新的制作槽
        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent); // 实例化新的制作槽
            newSlot.GetComponent<UI_CraftSlot>().SetuoCraftSlot(craftEquipment[i]); // 设置制作槽的装备信息
        }
    }

    // OnPointerDown方法在指针按下时调用，重新设置制作列表
    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList(); // 调用SetupCraftList方法
    }

    // SetupDefaultCraftWindow方法用于设置默认的制作窗口
    public void SetupDefaultCraftWindow()
    {
        // 如果第一个可制作装备不为空，则设置默认的制作窗口
        if (craftEquipment[0] != null)
        {
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]); // 设置制作窗口的装备信息
        }
    }
}
