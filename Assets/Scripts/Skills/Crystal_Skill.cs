using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间
using UnityEngine.UI; // 引入Unity UI命名空间

// Crystal_Skill类，继承自Skill类，用于创建和管理水晶技能
public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration; // 水晶持续时间
    [SerializeField] private GameObject crystalPrefab; // 水晶预制体
    private GameObject currentCrystal; // 当前水晶对象

    [Header("Crystal mirage")] // Unity编辑器中的分组标签
    [SerializeField] private UI_SkillTreeSlot unlockCloneInsteadButton; // 解锁克隆代替水晶的按钮
    [SerializeField] private bool cloneInsteadOfCrystal; // 是否使用克隆代替水晶的标志

    [Header("Crystal simple")] // Unity编辑器中的分组标签
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton; // 解锁水晶的按钮
    public bool crystalUnlocked { get; private set; } // 水晶是否解锁的标志

    [Header("Explosion crystal")] // Unity编辑器中的分组标签
    [SerializeField] private UI_SkillTreeSlot unlockExplosiveButton; // 解锁爆炸水晶的按钮
    [SerializeField] private float explosiveCooldown; // 爆炸冷却时间
    [SerializeField] private bool canExplode; // 是否可以爆炸的标志

    [Header("Moving crystal")] // Unity编辑器中的分组标签
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton; // 解锁移动水晶的按钮
    [SerializeField] private bool canMoveToEnemy; // 是否可以移动到敌人的标志
    [SerializeField] private float moveSpeed; // 移动速度

    [Header("Multi stacking crystal")] // Unity编辑器中的分组标签
    [SerializeField] private UI_SkillTreeSlot unlockMultiStackButton; // 解锁多重堆叠水晶的按钮
    [SerializeField] private bool canUseMultiStacks; // 是否可以使用多重堆叠的标志
    [SerializeField] private int amountOfStacks; // 堆叠数量
    [SerializeField] private float multiStackCooldown; // 多重堆叠冷却时间
    [SerializeField] private float useTimeWindow; // 使用时间窗口
    [SerializeField] List<GameObject> crystalLeft = new List<GameObject>(); // 剩余水晶列表

    // Start方法，在游戏开始时调用，用于初始化按钮监听事件
    protected override void Start()
    {
        base.Start(); // 调用基类的Start方法

        // 为解锁水晶按钮添加监听事件
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        // 为解锁克隆代替水晶按钮添加监听事件
        unlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        // 为解锁爆炸水晶按钮添加监听事件
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        // 为解锁移动水晶按钮添加监听事件
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        // 为解锁多重堆叠水晶按钮添加监听事件
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStack);
    }

    #region Unlock skill region
    // CheckUnlock方法，用于检查并解锁技能
    protected override void CheckUnlock()
    {
        UnlockCrystal(); // 解锁水晶
        UnlockCrystalMirage(); // 解锁克隆代替水晶
        UnlockExplosiveCrystal(); // 解锁爆炸水晶
        UnlockMovingCrystal(); // 解锁移动水晶
        UnlockMultiStack(); // 解锁多重堆叠水晶
    }

    // UnlockCrystal方法，用于解锁水晶
    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked) // 如果按钮已解锁
            crystalUnlocked = true; // 设置水晶已解锁
    }

    // UnlockCrystalMirage方法，用于解锁克隆代替水晶
    private void UnlockCrystalMirage()
    {
        if (unlockCloneInsteadButton.unlocked) // 如果按钮已解锁
            cloneInsteadOfCrystal = true; // 设置使用克隆代替水晶
    }

    // UnlockExplosiveCrystal方法，用于解锁爆炸水晶
    private void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveButton.unlocked) // 如果按钮已解锁
        {
            cooldown = explosiveCooldown; // 设置冷却时间
            canExplode = true; // 设置可以爆炸
        }
    }

    // UnlockMovingCrystal方法，用于解锁移动水晶
    private void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked) // 如果按钮已解锁
            canMoveToEnemy = true; // 设置可以移动到敌人
    }

    // UnlockMultiStack方法，用于解锁多重堆叠水晶
    private void UnlockMultiStack()
    {
        if (unlockMultiStackButton.unlocked) // 如果按钮已解锁
            canUseMultiStacks = true; // 设置可以使用多重堆叠
    }
    #endregion

    // UseSkill方法，用于使用技能
    public override void UseSkill()
    {
        base.UseSkill(); // 调用基类的UseSkill方法

        if (CanUseMultiCrystal()) // 如果可以使用多重水晶
        {
            return; // 返回
        }

        if (currentCrystal == null) // 如果当前没有水晶
        {
            CreateCrystal(); // 创建水晶
        }
        else
        {
            // 如果水晶可以移动到敌人，那么就不交换位置
            if (canMoveToEnemy)
            {
                return; // 返回
            }

            Vector2 playerPos = player.transform.position; // 获取玩家位置
            player.transform.position = currentCrystal.transform.position; // 将玩家位置设置为当前水晶位置
            currentCrystal.transform.position = playerPos; // 将当前水晶位置设置为玩家位置

            // 如果使用克隆代替水晶
            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero); // 创建克隆
                Destroy(currentCrystal); // 销毁当前水晶
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal(); // 完成水晶
            }
        }
    }

    // CreateCrystal方法，用于创建一个水晶
    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity); // 在玩家位置生成水晶
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>(); // 获取水晶控制器脚本
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform)); // 设置水晶
    }

    // CurrentCrystalChooseRandomTarget方法，用于让当前水晶选择一个随机目标
    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    // CanUseMultiCrystal方法，用于检查是否可以使用多重水晶
    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks) // 如果可以使用多重堆叠
        {
            if (crystalLeft.Count > 0) // 如果剩余水晶数量大于0
            {
                // 如果是开始使用的第一个水晶，在useTimeWindow时间内不再调用
                // 否则不强制等待生成水晶
                if (crystalLeft.Count == amountOfStacks)
                {
                    Invoke("ResetAbility", useTimeWindow); // 调用ResetAbility方法
                }

                cooldown = 0; // 设置冷却时间为0
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1]; // 从列表中获取最后一个水晶
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity); // 在玩家位置生成一个水晶
                crystalLeft.Remove(crystalToSpawn); // 从列表中移除该水晶
                Crystal_Skill_Controller currentCrystalScript = newCrystal.GetComponent<Crystal_Skill_Controller>(); // 获取水晶控制器脚本

                newCrystal.GetComponent<Crystal_Skill_Controller>()?.
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform)); // 设置水晶

                // 如果剩余水晶数量小于等于0
                if (crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown; // 设置多重堆叠冷却时间
                    RefilCrystal(); // 补充水晶
                }
            }
            return true; // 返回true
        }
        return false; // 返回false
    }

    // RefilCrystal方法，用于补充水晶
    private void RefilCrystal()
    {
        while (crystalLeft.Count < amountOfStacks) // 当剩余水晶数量小于堆叠数量时
        {
            crystalLeft.Add(crystalPrefab); // 添加水晶到列表中
        }
    }

    // ResetAbility方法，用于重置技能
    private void ResetAbility()
    {
        if (cooldownTimer > 0) // 如果冷却时间大于0
        {
            return; // 返回
        }
        cooldownTimer = multiStackCooldown; // 设置冷却时间为多重堆叠冷却时间
        RefilCrystal(); // 补充水晶
    }

}
