using UnityEngine; // 引入Unity引擎命名空间
using UnityEngine.EventSystems; // 引入事件系统命名空间，用于处理UI事件
using UnityEngine.UI; // 引入UI命名空间，用于处理UI组件

// UI_SkillTreeSlot类用于管理技能树中的技能槽
public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    private UI ui; // UI类的引用，用于访问UI组件
    private Image skillImage; // Image组件的引用，用于显示技能图标

    [SerializeField] private int skillCost; // 序列化字段，技能的花费
    [SerializeField] private string skillName; // 序列化字段，技能的名称
    [TextArea]
    [SerializeField] private string skillDescription; // 序列化字段，技能的描述
    [SerializeField] private Color lockedSkillColor; // 序列化字段，技能锁定时的颜色
    public bool unlocked; // 技能是否解锁的标志

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked; // 序列化字段，解锁当前技能前需要解锁的技能
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked; // 序列化字段，解锁当前技能前需要锁定的技能

    // OnValidate方法用于在编辑器中验证和更新对象的名称
    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName; // 更新对象名称为技能名称
    }

    // Awake方法在对象激活时调用，用于初始化组件
    private void Awake()
    {
        // 为按钮添加点击事件监听器，点击时调用UnlockSkillSlot方法
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    // Start方法在游戏开始时调用，用于初始化技能槽的状态
    private void Start()
    {
        skillImage = GetComponent<Image>(); // 获取Image组件
        ui = GetComponentInParent<UI>(); // 获取父对象中的UI组件
        skillImage.color = lockedSkillColor; // 设置技能图标为锁定颜色

        if (unlocked) { 
            skillImage.color = Color.white; // 如果技能已解锁，设置图标为白色
        }
    }

    // UnlockSkillSlot方法用于解锁技能槽
    public void UnlockSkillSlot()
    {
        // 检查所有前置技能是否已解锁
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (!shouldBeUnlocked[i].unlocked)
            {
                Debug.Log("Cannot unlock skill"); // 如果有未解锁的前置技能，输出日志并返回
                return;
            }
        }

        // 检查所有前置技能是否已锁定
        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked)
            {
                Debug.Log("Cannot unlock skill"); // 如果有未锁定的前置技能，输出日志并返回
                return;
            }
        }

        // 检查玩家是否有足够的金钱解锁技能
        if (PlayerManager.instance.HaveEnoughtMoney(skillCost) == false)
        {
            return; // 如果金钱不足，直接返回
        }

        unlocked = true; // 设置技能为已解锁
        skillImage.color = Color.white; // 设置技能图标为白色
    }

    // OnPointerEnter方法在鼠标指针进入技能槽时调用
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription, skillName, skillCost); // 显示技能提示信息
    }

    // OnPointerExit方法在鼠标指针离开技能槽时调用
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip(); // 隐藏技能提示信息
    }

    // LoadData方法用于从游戏数据中加载技能槽的状态
    public void LoadData(GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value; // 从数据中获取技能的解锁状态
        }
    }

    // SaveData方法用于将技能槽的状态保存到游戏数据中
    public void SaveData(ref GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName); // 移除旧的技能状态
            _data.skillTree.Add(skillName, unlocked); // 添加新的技能状态
        }
        else
        {
            _data.skillTree.Add(skillName, unlocked); // 如果不存在，直接添加技能状态
        }
    }
}
