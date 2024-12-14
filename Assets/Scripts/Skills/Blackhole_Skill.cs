using UnityEngine; // 引入Unity引擎命名空间
using UnityEngine.UI; // 引入Unity UI命名空间

// 黑洞技能类，继承自技能基类
public class Blackhole_Skill : Skill
{
    [SerializeField] private UI_SkillTreeSlot blackHoleUnlockButton; // 黑洞解锁按钮
    public bool blackholeUnlocked { get; private set; } // 黑洞是否解锁的标志
    [SerializeField] private int amountOfAttacks; // 攻击次数
    [SerializeField] private float cloneAttackCooldown; // 克隆攻击冷却时间
    [SerializeField] private float blackholeDuration; // 黑洞持续时间
    [Space]
    [SerializeField] private GameObject blackHolePrefab; // 黑洞预制体
    [SerializeField] private float maxSize; // 黑洞最大尺寸
    [SerializeField] private float growSpeed; // 黑洞增长速度
    [SerializeField] private float shrinkSpeed; // 黑洞缩小速度

    Blackhole_Skill_Controller currentBlackhole; // 当前黑洞控制器

    // 初始化方法，设置解锁按钮的监听事件
    protected override void Start()
    {
        base.Start(); // 调用基类的Start方法
        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackhole); // 添加解锁黑洞的监听事件
    }

    // 更新方法，调用基类的Update方法
    protected override void Update()
    {
        base.Update(); // 调用基类的Update方法
    }

    // 解锁黑洞的方法
    private void UnlockBlackhole()
    {
        if (blackHoleUnlockButton.unlocked) // 如果按钮已解锁
            blackholeUnlocked = true; // 设置黑洞已解锁
    }

    // 检查技能是否可以使用
    public override bool CanUseSkill()
    {
        return base.CanUseSkill(); // 调用基类的CanUseSkill方法
    }

    // 使用技能的方法
    public override void UseSkill()
    {
        base.UseSkill(); // 调用基类的UseSkill方法
        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity); // 在玩家位置生成黑洞

        currentBlackhole = newBlackHole.GetComponent<Blackhole_Skill_Controller>(); // 获取黑洞控制器
        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown, blackholeDuration); // 设置黑洞参数
    }

    // 检查技能是否完成
    public bool SkillCompleted()
    {
        if (!currentBlackhole) // 如果当前没有黑洞
        {
            return false; // 返回未完成
        }

        if (currentBlackhole.playerCanExitState) // 如果玩家可以退出状态
        {
            currentBlackhole = null; // 清空当前黑洞
            return true; // 返回完成
        }
        return false; // 返回未完成
    }

    // 获取黑洞半径
    public float GetBlackholeRadius()
    {
        return maxSize / 2; // 返回黑洞半径为最大尺寸的一半
    }

    // 检查解锁状态
    protected override void CheckUnlock()
    {
        base.CheckUnlock(); // 调用基类的CheckUnlock方法
        UnlockBlackhole(); // 调用解锁黑洞的方法
    }
}
