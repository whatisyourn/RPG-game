// 引入Unity引擎命名空间
using UnityEngine;
// 引入Unity UI命名空间
using UnityEngine.UI;

// Parry_Skill类继承自Skill类，用于实现格挡技能
public class Parry_Skill : Skill
{
    // 格挡相关的UI元素
    [Header("Parry")]
    // 格挡解锁按钮
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    // 格挡是否解锁的标志
    public bool parryUnlocked { get; private set; }

    // 格挡恢复相关的UI元素
    [Header("Parry restore")]
    // 恢复解锁按钮
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    // 恢复生命值的百分比
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPercentage;
    // 恢复是否解锁的标志
    public bool restoreUnlocked { get; private set; }

    // 幻影格挡相关的UI元素
    [Header("Parry with mirage")]
    // 幻影格挡解锁按钮
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    // 幻影格挡是否解锁的标志
    public bool parryWithMirageUnlocked { get; private set; }

    // 使用技能的方法，调用基类的UseSkill方法
    public override void UseSkill()
    {
        base.UseSkill(); // 调用基类的UseSkill方法
        if (restoreUnlocked) // 如果恢复已解锁
        {
            // 计算恢复的生命值
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPercentage);
            // 增加玩家的生命值
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    // 初始化方法，设置解锁按钮的监听事件
    protected override void Start()
    {
        base.Start(); // 调用基类的Start方法
        // 为格挡解锁按钮添加监听事件
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        // 为恢复解锁按钮添加监听事件
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        // 为幻影格挡解锁按钮添加监听事件
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    // 检查技能解锁状态的方法
    protected override void CheckUnlock()
    {
        UnlockParry(); // 检查并解锁格挡
        UnlockParryRestore(); // 检查并解锁恢复
        UnlockParryWithMirage(); // 检查并解锁幻影格挡
    }

    // 解锁格挡的方法
    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked) // 如果按钮已解锁
            parryUnlocked = true; // 设置格挡已解锁
    }

    // 解锁恢复的方法
    private void UnlockParryRestore()
    {
        if (restoreUnlockButton.unlocked) // 如果按钮已解锁
            restoreUnlocked = true; // 设置恢复已解锁
    }

    // 解锁幻影格挡的方法
    private void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked) // 如果按钮已解锁
            parryWithMirageUnlocked = true; // 设置幻影格挡已解锁
    }

    // 在格挡时创建幻影的方法
    public void MakeMirageOnParry(Transform respawnTransform)
    {
        if (parryWithMirageUnlocked) // 如果幻影格挡已解锁
            SkillManager.instance.clone.CreateCloneWithDelay(respawnTransform); // 创建延迟幻影
    }
}
