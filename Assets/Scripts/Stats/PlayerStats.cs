using UnityEngine; // 引入Unity引擎命名空间

// PlayerStats类继承自CharacterStats类，管理玩家的属性和行为
public class PlayerStats : CharacterStats
{
    private Player player; // 玩家对象，用于访问玩家的属性和方法

    // Start方法在对象被创建时调用，用于初始化玩家对象
    protected override void Start()
    {
        base.Start(); // 调用基类的Start方法
        player = PlayerManager.instance.player; // 从PlayerManager中获取玩家对象
    }

    // TakeDamage方法用于处理玩家受到的伤害
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage); // 调用基类的TakeDamage方法处理伤害
    }

    // Die方法用于处理玩家死亡时的逻辑
    protected override void Die()
    {
        base.Die(); // 调用基类的Die方法
        player.Die(); // 调用玩家的Die方法
        GameManager.instance.PauseGame(false); // 暂停游戏
        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency; // 记录玩家失去的货币数量
        PlayerManager.instance.currency = 0; // 重置玩家的货币数量
        GetComponent<PlayerItemDrop>().GenerateDrop(); // 生成玩家掉落物品
        SaveManager.instance.SaveGame(); // 保存游戏状态
    }

    // DecreaseHealthBy方法用于减少玩家的生命值
    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage); // 调用基类的DecreaseHealthBy方法

        // 如果受到的伤害超过最大生命值的30%，则触发击退效果
        if (_damage > GetMaxHealthValue() * .3f)
        {
            player.SetupKnockbackPower(new Vector2(10, 6)); // 设置击退力量
            player.fx.ScreenShake(player.fx.shakeHighDamage); // 触发屏幕震动效果
        }

        // 施加护甲效果
        ItemData_Equipment currenArmor = Inventory.instance.GetEquipment(EquipmentType.Armor); // 获取当前装备的护甲
        if (currenArmor != null)
        {
            currenArmor.Effect(player.transform); // 施加护甲效果
        }
    }

    // OnEvasion方法用于处理玩家闪避时的逻辑
    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge(); // 创建闪避时的幻影效果
    }

    // CloneDoDamage方法用于克隆对目标造成伤害
    public void CloneDoDamage(CharacterStats _targetStats, float _multiplier)
    {
        // 检查目标是否可以闪避攻击
        if (TargetCanAvoidAttack(_targetStats))
        {
            return; // 如果可以闪避，则不造成伤害
        }
        int totalDamage = damage.GetValue() + strength.GetValue(); // 计算总伤害值
        if (_multiplier > 0)
        {
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier); // 如果有倍率，则应用倍率
        }
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage); // 如果可以暴击，则计算暴击伤害
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage); // 检查目标护甲并调整伤害
        _targetStats.TakeDamage(totalDamage); // 对目标造成伤害
        DoMagicalDamage(_targetStats); // 造成魔法伤害，若目标死亡则施加额外效果
    }
}
