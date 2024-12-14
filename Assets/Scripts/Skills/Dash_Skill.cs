// 引入Unity引擎命名空间
using UnityEngine;
// 引入Unity UI命名空间
using UnityEngine.UI;

// Dash_Skill类继承自Skill类，用于实现冲刺技能
public class Dash_Skill : Skill
{
    // 冲刺相关的UI元素
    [Header("Dash")]
    // 冲刺解锁按钮
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;
    // 冲刺是否解锁的标志
    public bool dashUnlocked { get; private set; }

    // 冲刺时生成克隆相关的UI元素
    [Header("Clone on dash")]
    // 冲刺时克隆解锁按钮
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;
    // 冲刺时克隆是否解锁的标志
    public bool cloneOnDashUnlocked {  get; private set; }

    // 到达时生成克隆相关的UI元素
    [Header("Clone on arrival")]
    // 到达时克隆解锁按钮
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockedButton;
    // 到达时克隆是否解锁的标志
    public bool cloneOnArrivalUnlocked {  get; private set; }

    // 使用技能的方法，调用基类的UseSkill方法
    public override void UseSkill()
    {
        base.UseSkill(); // 调用基类的UseSkill方法
    }

    // 初始化方法，设置解锁按钮的监听事件
    protected override void Start()
    {
        base.Start(); // 调用基类的Start方法
        // 为冲刺解锁按钮添加监听事件
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        // 为冲刺时克隆解锁按钮添加监听事件
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        // 为到达时克隆解锁按钮添加监听事件
        cloneOnArrivalUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    // 检查技能解锁状态的方法
    protected override void CheckUnlock()
    {
        UnlockDash(); // 检查并解锁冲刺
        UnlockCloneOnDash(); // 检查并解锁冲刺时克隆
        UnlockCloneOnArrival(); // 检查并解锁到达时克隆
    }

    // 解锁冲刺的方法
    private void UnlockDash()
    {
        // 如果冲刺解锁按钮已解锁
        if (dashUnlockButton.unlocked)
        {
            dashUnlocked = true; // 设置冲刺已解锁
        }
    }

    // 解锁冲刺时克隆的方法
    private void UnlockCloneOnDash()
    {
        // 如果冲刺时克隆解锁按钮已解锁
        if (cloneOnDashUnlockButton.unlocked)
            cloneOnDashUnlocked = true; // 设置冲刺时克隆已解锁
    }

    // 解锁到达时克隆的方法
    private void UnlockCloneOnArrival()
    {
        // 如果到达时克隆解锁按钮已解锁
        if (cloneOnArrivalUnlockedButton.unlocked)
            cloneOnArrivalUnlocked = true; // 设置到达时克隆已解锁
    }

    // 在冲刺时生成克隆的方法
    public void CloneOnDash()
    {
        // 如果冲刺时克隆已解锁
        if (cloneOnDashUnlocked)
        {
            // 创建克隆对象
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }

    // 在到达时生成克隆的方法
    public void CloneOnArrival()
    {
        // 如果到达时克隆已解锁
        if (cloneOnArrivalUnlocked)
        {
            // 创建克隆对象
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }
}
