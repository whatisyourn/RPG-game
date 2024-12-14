using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using Unity.VisualScripting.Antlr3.Runtime.Misc; // 引入VisualScripting的Antlr3命名空间
using UnityEngine; // 引入Unity引擎命名空间

// EnemyStats类继承自CharacterStats类，用于管理敌人的属性和行为
public class EnemyStats : CharacterStats
{
    private Enemy enemy; // 敌人对象的引用
    private ItemDrop myDropSystem; // 掉落系统的引用
    public Stat soulsDropAmount; // 灵魂掉落数量的属性

    [Header("Level details")] // 在Unity编辑器中显示的分组标签
    [SerializeField] private int level; // 敌人的等级

    [Range(0f, 1f)] // 在Unity编辑器中限制值的范围
    [SerializeField] private float percentageModifier; // 属性修正百分比

    // Start方法在对象被创建时调用，用于初始化敌人的属性
    protected override void Start()
    {
        soulsDropAmount.SetDefaultValue(100); // 设置灵魂掉落数量的默认值为100
        ApplyLevelModifiers(); // 应用等级修正
        base.Start(); // 调用基类的Start方法
        enemy = GetComponent<Enemy>(); // 获取Enemy组件
        myDropSystem = GetComponent<ItemDrop>(); // 获取ItemDrop组件
    }

    // ApplyLevelModifiers方法用于根据等级修正敌人的属性
    private void ApplyLevelModifiers()
    {
        Modify(strength); // 修正力量属性
        Modify(agility); // 修正敏捷属性
        Modify(intelligence); // 修正智力属性
        Modify(vitality); // 修正体力属性

        Modify(damage); // 修正伤害属性
        // Modify(critChance); // 修正暴击几率属性（注释掉）
        // Modify(critPower); // 修正暴击威力属性（注释掉）

        Modify(maxHealth); // 修正最大生命值属性
        Modify(armor); // 修正护甲属性
        Modify(evasion); // 修正闪避属性
        Modify(magicResistance); // 修正魔法抗性属性

        Modify(fireDamage); // 修正火焰伤害属性
        Modify(iceDamage); // 修正冰霜伤害属性
        Modify(lightingDamage); // 修正闪电伤害属性

        Modify(soulsDropAmount); // 修正灵魂掉落数量属性
    }

    // Modify方法用于根据百分比修正指定的属性
    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++) // 从等级1开始循环到当前等级
        {
            float modifier = _stat.GetValue() * percentageModifier; // 计算修正值
            _stat.AddModifier(Mathf.RoundToInt(modifier)); // 将修正值添加到属性中
        }
    }

    // TakeDamage方法用于处理敌人受到的伤害
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage); // 调用基类的TakeDamage方法
    }

    // Die方法用于处理敌人死亡时的行为
    protected override void Die()
    {
        base.Die(); // 调用基类的Die方法
        enemy.Die(); // 调用敌人的Die方法
        PlayerManager.instance.currency += soulsDropAmount.GetValue(); // 增加玩家的货币数量
        myDropSystem.GenerateDrop(); // 生成掉落物品

        Destroy(gameObject, .5f); // 在0.5秒后销毁敌人对象
    }

}
