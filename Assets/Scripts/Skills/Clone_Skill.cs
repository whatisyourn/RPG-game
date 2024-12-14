using System.Collections; // 引入系统集合命名空间
using UnityEngine; // 引入Unity引擎命名空间
using UnityEngine.UI; // 引入Unity UI命名空间

// Clone_Skill类继承自Skill类，用于创建和管理克隆技能
public class Clone_Skill : Skill
{
    // 克隆信息的字段
    [Header("Clone info")] // Unity编辑器中的分组标签
    [SerializeField] private float attackMultiplier; // 攻击倍率
    [SerializeField] private GameObject clonePrefab; // 克隆的预制体，用于实例化克隆对象
    [SerializeField] private float cloneDuration; // 克隆的持续时间
    [Space] // 在Unity编辑器中插入空行

    [Header("Clone attack")] // 克隆攻击的相关字段
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton; // 克隆攻击解锁按钮
    [SerializeField] private float cloneAttackMultiplier; // 克隆攻击倍率
    [SerializeField] private bool canAttack; // 克隆是否可以攻击

    [Header("Aggresive clone")] // 激进克隆的相关字段
    [SerializeField] private UI_SkillTreeSlot aggresiveCloneUnlockButton; // 激进克隆解锁按钮
    [SerializeField] private float aggresiveCloneAttackMultiplier; // 激进克隆攻击倍率
    public bool canApplyOnHitEffect { get; private set; } // 是否可以应用命中效果

    [Header("Multiple clone")] // 多重克隆的相关字段
    [SerializeField] private UI_SkillTreeSlot multipleUnlockButton; // 多重克隆解锁按钮
    [SerializeField] private float multiCloneAttackMultiplier; // 多重克隆攻击倍率
    [SerializeField] private bool canDuplicateClone; // 是否可以复制克隆
    [SerializeField] private float chanceToDuplicate; // 复制克隆的几率

    [Header("Crystal insteal of clone")] // 替代克隆的水晶相关字段
    [SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockButton; // 水晶替代解锁按钮
    public bool crystalInstalOfClone; // 是否用水晶替代克隆

    // 初始化方法，设置按钮监听器
    protected override void Start()
    {
        base.Start(); // 调用基类的Start方法
        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack); // 添加克隆攻击解锁监听器
        aggresiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone); // 添加激进克隆解锁监听器
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone); // 添加多重克隆解锁监听器
        crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead); // 添加水晶替代解锁监听器
    }

    #region Unlock region
    // 检查解锁状态的方法
    protected override void CheckUnlock()
    {
        UnlockCloneAttack(); // 检查并解锁克隆攻击
        UnlockAggresiveClone(); // 检查并解锁激进克隆
        UnlockMultiClone(); // 检查并解锁多重克隆
        UnlockCrystalInstead(); // 检查并解锁水晶替代
    }

    // 解锁克隆攻击的方法
    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked) // 如果按钮已解锁
        {
            canAttack = true; // 设置可以攻击
            attackMultiplier = cloneAttackMultiplier; // 设置攻击倍率
        }
    }

    // 解锁激进克隆的方法
    private void UnlockAggresiveClone()
    {
        if (aggresiveCloneUnlockButton.unlocked) // 如果按钮已解锁
        {
            canApplyOnHitEffect = true; // 设置可以应用命中效果
            attackMultiplier = aggresiveCloneAttackMultiplier; // 设置攻击倍率
        }
    }

    // 解锁多重克隆的方法
    private void UnlockMultiClone()
    {
        if (multipleUnlockButton.unlocked) // 如果按钮已解锁
        {
            canDuplicateClone = true; // 设置可以复制克隆
            attackMultiplier = multiCloneAttackMultiplier; // 设置攻击倍率
        }
    }

    // 解锁水晶替代的方法
    private void UnlockCrystalInstead()
    {
        if (crystalInsteadUnlockButton.unlocked) // 如果按钮已解锁
        {
            crystalInstalOfClone = true; // 设置用水晶替代克隆
        }
    }
    #endregion

    // 创建克隆的方法
    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInstalOfClone) // 如果用水晶替代克隆
        {
            SkillManager.instance.crystal.CreateCrystal(); // 创建水晶
            return; // 结束方法
        }

        // 实例化克隆预制体，创建一个新的克隆对象
        GameObject newClone = Instantiate(clonePrefab);

        // 获取新克隆对象上的Clone_Skill_Controller组件，并设置克隆的行为
        newClone.GetComponent<Clone_Skill_Controller>().
            SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate, player, attackMultiplier);
    }

    // 创建带延迟的克隆的方法
    public void CreateCloneWithDelay(Transform _enemy)
    {
        // 启动协程，在延迟后创建克隆
        StartCoroutine(CloneDelayCorotine(_enemy, new Vector3(2 * player.facingDir, 0)));
    }

    // 克隆延迟协程
    private IEnumerator CloneDelayCorotine(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f); // 等待0.4秒
        CreateClone(_transform, _offset); // 创建克隆
    }
}