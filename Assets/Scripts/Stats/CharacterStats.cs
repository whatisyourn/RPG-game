using System.Collections; // 引入系统集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// 定义一个枚举类型StatType，表示不同的属性类型
public enum StatType
{
    strength, // 力量
    agility, // 敏捷
    intelligence, // 智力
    vitality, // 体力
    damage, // 伤害
    critChance, // 暴击几率
    critPower, // 暴击伤害
    health, // 生命值
    armor, // 护甲
    evasion, // 闪避
    magicResistance, // 魔法抗性
    fireDamage, // 火焰伤害
    iceDamage, // 冰霜伤害
    lightingDamage // 闪电伤害
}

// 定义CharacterStats类，继承自MonoBehaviour
public class CharacterStats : MonoBehaviour
{
    private EntityFX fx; // 实体特效

    [Header("Major stats")]
    public Stat strength; // 力量，每1点力量增加1%的暴击伤害
    public Stat agility; // 敏捷，每1点敏捷增加1%的暴击几率
    public Stat intelligence; // 智力，每1点智力增加1点魔法伤害和3点魔法抗性
    public Stat vitality; // 体力，每1点体力增加5点生命值

    [Header("Offensive stats")]
    public Stat damage; // 伤害
    public Stat critChance; // 暴击几率
    public Stat critPower; // 暴击伤害，默认值为150%

    [Header("Defensive stats")]
    public Stat maxHealth; // 最大生命值
    public Stat armor; // 护甲
    public Stat evasion; // 闪避
    public Stat magicResistance; // 魔法抗性

    [Header("Magic stats")]
    public Stat fireDamage; // 火焰伤害
    public Stat iceDamage; // 冰霜伤害
    public Stat lightingDamage; // 闪电伤害

    public bool isIgnited; // 是否被点燃
    public bool isChilled; // 是否被冰冻
    public bool isShocked; // 是否被电击

    [SerializeField] private float ailmentsDuration = 4; // 状态持续时间
    private float ignitedTimer; // 点燃计时器
    private float chilledTimer; // 冰冻计时器
    private float shockedTimer; // 电击计时器

    private float igniteDamageCooldown = .3f; // 点燃伤害冷却时间
    private float igniteDamageTimer; // 点燃伤害计时器
    private int igniteDamage; // 点燃伤害值
    [SerializeField] private GameObject shockStrickPrefab; // 电击预制体
    private int shockDamage; // 电击伤害值
    public int currentHealth; // 当前生命值

    public System.Action onHealthChanged; // 生命值变化时的回调
    public bool isDead { get; private set; } // 是否死亡
    public bool isInvincible { get; private set; } // 是否无敌
    private bool isVulnerable; // 是否脆弱

    // 初始化方法，设置初始状态
    protected virtual void Start()
    {
        currentHealth = GetMaxHealthValue(); // 获取最大生命值
        critPower.SetDefaultValue(150); // 设置默认暴击伤害为150%
        fx = GetComponent<EntityFX>(); // 获取EntityFX组件
    }

    // 更新方法，每帧调用一次
    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime; // 减少点燃计时器
        chilledTimer -= Time.deltaTime; // 减少冰冻计时器
        shockedTimer -= Time.deltaTime; // 减少电击计时器

        igniteDamageTimer -= Time.deltaTime; // 减少点燃伤害计时器

        if (ignitedTimer < 0) // 如果点燃计时器小于0
        {
            isIgnited = false; // 取消点燃状态
        }

        if (chilledTimer < 0) // 如果冰冻计时器小于0
        {
            isChilled = false; // 取消冰冻状态
        }

        if (shockedTimer < 0) // 如果电击计时器小于0
        {
            isShocked = false; // 取消电击状态
        }
        if (isIgnited) // 如果处于点燃状态
            ApplyIgniteDamage(); // 应用点燃伤害
    }

    // 使角色在指定时间内变得脆弱
    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableForCorutine(_duration));

    // 协程：使角色在指定时间内变得脆弱
    private IEnumerator VulnerableForCorutine(float _duartion)
    {
        isVulnerable = true; // 设置脆弱状态
        yield return new WaitForSeconds(_duartion); // 等待指定时间
        isVulnerable = false; // 取消脆弱状态
    }

    // 增加指定属性的值，并在指定时间后恢复
    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }

    // 协程：增加指定属性的值，并在指定时间后恢复
    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier); // 增加属性值
        yield return new WaitForSeconds(_duration); // 等待指定时间
        _statToModify.RemoveModifier(_modifier); // 恢复属性值
    }

    // 对目标角色造成伤害
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        bool cirticalStrike = false; // 是否暴击

        if (TargetCanAvoidAttack(_targetStats)) // 如果目标可以闪避攻击
        {
            return; // 直接返回
        }

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform); // 设置目标的击退方向

        int totalDamage = damage.GetValue() + strength.GetValue(); // 计算总伤害
        if (CanCrit()) // 如果可以暴击
        {
            totalDamage = CalculateCriticalDamage(totalDamage); // 计算暴击伤害
            cirticalStrike = true; // 设置暴击标志
        }

        fx.CreateHitFx(_targetStats.transform, cirticalStrike); // 创建命中特效
        totalDamage = CheckTargetArmor(_targetStats, totalDamage); // 检查目标护甲
        _targetStats.TakeDamage(totalDamage); // 对目标造成伤害
        DoMagicalDamage(_targetStats); // 对目标造成魔法伤害
    }

    #region Magical damage and ailments
    // 对目标角色造成魔法伤害
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue(); // 获取火焰伤害
        int _iceDamage = iceDamage.GetValue(); // 获取冰霜伤害
        int _lightDamage = lightingDamage.GetValue(); // 获取闪电伤害

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightDamage + intelligence.GetValue(); // 计算总魔法伤害

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage); // 检查目标魔法抗性
        _targetStats.TakeDamage(totalMagicalDamage); // 对目标造成魔法伤害

        if (Mathf.Max(_fireDamage, _iceDamage, _lightDamage) <= 0) // 如果所有魔法伤害都小于等于0
        {
            return; // 直接返回
        }
        AttemptToApplyAilements(_targetStats, _fireDamage, _iceDamage, _lightDamage); // 尝试应用异常状态
    }

    // 尝试对目标角色应用异常状态
    private void AttemptToApplyAilements(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightDamage; // 是否可以应用点燃状态
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightDamage; // 是否可以应用冰冻状态
        bool canApplyShock = _lightDamage > _fireDamage && _lightDamage > _iceDamage; // 是否可以应用电击状态

        while (!canApplyIgnite && !canApplyChill && !canApplyShock) // 如果没有任何异常状态可以应用
        {
            if (Random.value < .3f && _fireDamage > 0) // 30%的几率应用点燃状态
            {
                canApplyIgnite = true; // 设置点燃状态
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // 应用异常状态
                return; // 直接返回
            }
            if (Random.value < .4f && _iceDamage > 0) // 40%的几率应用冰冻状态
            {
                canApplyChill = true; // 设置冰冻状态
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // 应用异常状态
                return; // 直接返回
            }
            if (Random.value < .5f && _lightDamage > 0) // 50%的几率应用电击状态
            {
                canApplyShock = true; // 设置电击状态
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // 应用异常状态
                return; // 直接返回
            }
        }

        if (canApplyIgnite) // 如果可以应用点燃状态
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f)); // 设置点燃伤害
        }

        if (canApplyShock) // 如果可以应用电击状态
        {
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightDamage * .1f)); // 设置电击伤害
        }
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // 应用异常状态
    }

    // 应用异常状态
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked; // 是否可以应用点燃状态
        bool canApplyChill = !isIgnited && !isChilled && !isShocked; // 是否可以应用冰冻状态
        bool canApplyShock = !isIgnited && !isChilled; // 是否可以应用电击状态

        if (_ignite && canApplyIgnite) // 如果可以应用点燃状态
        {
            isIgnited = _ignite; // 设置点燃状态
            ignitedTimer = ailmentsDuration; // 设置点燃计时器
            fx.IgniteFXFor(ailmentsDuration); // 应用点燃特效
        }

        else if (_chill && canApplyChill) // 如果可以应用冰冻状态
        {
            isChilled = _chill; // 设置冰冻状态
            chilledTimer = ailmentsDuration; // 设置冰冻计时器

            float slowPercentage = .2f; // 减速百分比
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration); // 减速实体
            fx.ChillFXFor(ailmentsDuration); // 应用冰冻特效
        }
        else if (_shock && canApplyShock) // 如果可以应用电击状态
        {
            if (!isShocked) // 如果当前没有电击状态
            {
                ApplyShock(_shock); // 应用电击状态
            }
            else // 如果当前有电击状态
            {
                if (GetComponent<Player>() != null) // 如果是玩家
                    return; // 直接返回

                HitNearestTargetWithShockStrike(); // 电击最近的目标
            }
        }
    }

    // 应用电击状态
    public void ApplyShock(bool _shock)
    {
        if (isShocked) // 如果当前有电击状态
        {
            return; // 直接返回
        }
        isShocked = _shock; // 设置电击状态
        shockedTimer = ailmentsDuration; // 设置电击计时器
        fx.ShockFXFor(ailmentsDuration); // 应用电击特效
    }

    // 应用点燃伤害
    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0) // 如果点燃伤害计时器小于0
        {
            DecreaseHealthBy(igniteDamage); // 减少生命值

            if (currentHealth < 0 && !isDead) // 如果生命值小于0且未死亡
            {
                Die(); // 角色死亡
            }

            igniteDamageTimer = igniteDamageCooldown; // 重置点燃伤害计时器
        }
    }

    // 电击最近的目标
    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25); // 获取范围内的所有碰撞体

        float closestDistance = Mathf.Infinity; // 初始化最近距离为无穷大
        Transform closestEnemy = null; // 最近的敌人

        foreach (var hit in colliders) // 遍历所有碰撞体
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1) // 如果是敌人且距离大于1
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position); // 计算距离
                if (distanceToEnemy < closestDistance) // 如果距离小于最近距离
                {
                    closestEnemy = hit.transform; // 设置最近的敌人
                    closestDistance = distanceToEnemy; // 更新最近距离
                }
            }
        }

        if (closestEnemy == null) // 如果没有找到敌人
        {
            closestEnemy = transform; // 设置自己为最近的敌人
        }

        if (closestEnemy != null) // 如果找到敌人
        {
            GameObject newShockStrike = Instantiate(shockStrickPrefab, transform.position, Quaternion.identity); // 实例化电击预制体

            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>()); // 设置电击伤害
        }
    }

    // 设置点燃伤害
    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

    // 设置电击伤害
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;
    #endregion

    // 角色受到伤害
    public virtual void TakeDamage(int _damage)
    {
        if (isInvincible) // 如果无敌
        {
            return; // 直接返回
        }

        DecreaseHealthBy(_damage); // 减少生命值

        GetComponent<Entity>().DamageImpact(); // 触发受击效果
        fx.StartCoroutine("FlashFX"); // 触发闪烁特效

        if (currentHealth < 0 && !isDead) // 如果生命值小于0且未死亡
        {
            Die(); // 角色死亡
        }
    }

    // 增加生命值
    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount; // 增加生命值
        if (currentHealth > GetMaxHealthValue()) // 如果生命值超过最大值
        {
            currentHealth = GetMaxHealthValue(); // 设置为最大值
        }

        if (onHealthChanged != null) // 如果有生命值变化回调
        {
            onHealthChanged(); // 触发回调
        }
    }

    // 减少生命值
    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable) // 如果脆弱
        {
            _damage = Mathf.RoundToInt(_damage * 1.1f); // 增加伤害
        }
        currentHealth -= _damage; // 减少生命值

        if(_damage > 0) // 如果伤害大于0
        {
            fx.CreatePopUpText(_damage.ToString()); // 创建伤害弹出文本
        }

        if (onHealthChanged != null) // 如果有生命值变化回调
        {
            onHealthChanged(); // 触发回调
        }
    }

    // 角色死亡
    protected virtual void Die()
    {
        isDead = true; // 设置死亡状态
    }

    // 杀死角色
    public void KillEntity()
    {
        if (!isDead) // 如果未死亡
        {
            Die(); // 角色死亡
        }
    }

    // 设置无敌状态
    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;

    #region Stat calculations

    // 闪避时的处理
    public virtual void OnEvasion()
    {

    }

    // 检查目标是否可以闪避攻击
    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEavsion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue(); // 计算总闪避值

        if (isShocked) // 如果被电击
        {
            totalEavsion += 20; // 增加20点闪避值
        }
        if (Random.Range(0, 100) < totalEavsion) // 如果随机值小于总闪避值
        {
            _targetStats.OnEvasion(); // 触发闪避处理
            return true; // 返回true
        }
        return false; // 返回false
    }

    // 检查目标护甲
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled) // 如果目标被冰冻
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f); // 减少80%的护甲值
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue(); // 减少护甲值
        }
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue); // 限制伤害值在0到int最大值之间
        return totalDamage; // 返回总伤害值
    }

    // 检查目标魔法抗性
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3); // 减少魔法抗性和智力值的3倍
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue); // 限制魔法伤害值在0到int最大值之间
        return totalMagicalDamage; // 返回总魔法伤害值
    }

    // 检查是否可以暴击
    protected bool CanCrit()
    {
        int totalCritialChance = critChance.GetValue() + agility.GetValue(); // 计算总暴击几率
        if (Random.Range(0, 100) <= totalCritialChance) // 如果随机值小于等于总暴击几率
        {
            return true; // 返回true
        }
        return false; // 返回false
    }

    // 计算暴击伤害
    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (strength.GetValue() + critPower.GetValue()) * .01f; // 计算总暴击伤害

        float critDamage = _damage * totalCritPower; // 计算暴击伤害值

        return Mathf.RoundToInt(critDamage); // 返回四舍五入后的暴击伤害值
    }

    // 获取最大生命值
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5; // 返回最大生命值
    }
    #endregion

    // 获取指定类型的属性
    public Stat GetStat(StatType _statType)
    {
        switch (_statType) // 根据属性类型返回对应的属性
        {
            case StatType.strength: return strength;
            case StatType.agility: return agility;
            case StatType.intelligence: return intelligence;
            case StatType.vitality: return vitality;
            case StatType.damage: return damage;
            case StatType.critChance: return critChance;
            case StatType.critPower: return critPower;
            case StatType.health: return maxHealth;
            case StatType.armor: return armor;
            case StatType.evasion: return evasion;
            case StatType.magicResistance: return magicResistance;
            case StatType.fireDamage: return fireDamage;
            case StatType.iceDamage: return iceDamage;
            case StatType.lightingDamage: return lightingDamage;
            default: return null;
        }
    }
}
