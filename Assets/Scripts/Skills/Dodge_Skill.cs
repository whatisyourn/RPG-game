// 引入Unity引擎命名空间
using UnityEngine;
// 引入Unity UI命名空间
using UnityEngine.UI;

// Dodge_Skill类继承自Skill类，用于实现闪避技能
public class Dodge_Skill : Skill
{
    // 闪避相关的UI元素
    [Header("Dodge")]
    // 闪避解锁按钮
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    // 闪避增加的闪避值
    [SerializeField] private int evasionAmount;
    // 闪避是否解锁的标志
    public bool dodgeUnlocked;
    
    // 幻影闪避相关的UI元素
    [Header("Mirage dodge")]
    // 幻影闪避解锁按钮
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodge;
    // 幻影闪避是否解锁的标志
    public bool dodgeMirageUnlocked;

    // 初始化方法，设置按钮监听器
    protected override void Start()
    {
        base.Start(); // 调用基类的Start方法
        // 为闪避解锁按钮添加监听器
        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        // 为幻影闪避解锁按钮添加监听器
        unlockMirageDodge.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    // 检查解锁状态的方法
    protected override void CheckUnlock()
    {
        UnlockDodge(); // 检查并解锁闪避
        UnlockMirageDodge(); // 检查并解锁幻影闪避
    }

    // 解锁闪避的方法
    private void UnlockDodge()
    {
        // 如果闪避按钮已解锁且闪避未解锁
        if (unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            // 增加玩家的闪避值
            player.stats.evasion.AddModifier(evasionAmount);
            // 更新UI中的状态
            Inventory.instance.UpdateStatUI();
            // 设置闪避已解锁
            dodgeUnlocked = true;
        }
    }

    // 解锁幻影闪避的方法
    private void UnlockMirageDodge()
    {
        // 如果幻影闪避按钮已解锁
        if (unlockMirageDodge.unlocked)
        {
            // 设置幻影闪避已解锁
            dodgeMirageUnlocked = true;
        }
    }

    // 在闪避时创建幻影的方法
    public void CreateMirageOnDodge()
    {
        // 如果幻影闪避已解锁
        if (dodgeMirageUnlocked)
            // 创建一个新的克隆对象
            SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDir,0));
    }
}
