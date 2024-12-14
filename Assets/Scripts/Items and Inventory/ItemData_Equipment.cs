using System.Collections.Generic; // 引入系统集合泛型命名空间
using UnityEngine; // 引入Unity引擎命名空间

// 定义一个枚举类型，用于表示装备类型
public enum EquipmentType
{
    Weapon, // 武器
    Armor, // 护甲
    Amulet, // 护身符
    Flask // 药瓶
}

// 使用CreateAssetMenu属性创建一个可在Unity编辑器中创建的装备数据脚本对象
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType; // 定义一个变量，用于存储装备类型

    [Header("Unique effect")]
    public float itemCooldown; // 定义一个浮点变量，用于存储物品冷却时间
    public ItemEffect[] itemEffects; // 定义一个数组，用于存储物品效果

    [Header("Major stats")]
    public int strength; // 力量，每1点增加1%暴击伤害
    public int agility; // 敏捷，每1点增加1%暴击几率
    public int intelligence; // 智力，每1点增加1点魔法伤害和3点魔法抗性
    public int vitality; // 体力，每1点增加3点生命值

    [Header("Offensive stats")]
    public int damage; // 伤害
    public int critChance; // 暴击几率
    public int critPower; // 暴击伤害，默认150%

    [Header("Defensive stats")]
    public int maxHealth; // 最大生命值
    public int armor; // 护甲
    public int evasion; // 闪避
    public int magicResistance; // 魔法抗性

    [Header("Magic stats")]
    public int fireDamage; // 火焰伤害
    public int iceDamage; // 冰霜伤害
    public int lightingDamage; // 闪电伤害

    [Header("Craft requirements")]
    public List<InventoryItem> craftMaterials; // 定义一个列表，用于存储制作材料

    private int descriptionLength; // 定义一个变量，用于存储描述的长度

    // 执行物品效果的方法，遍历所有效果并执行
    public void Effect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects) // 遍历每个物品效果
        {
            item.ExecuteEffect(_enemyPosition); // 执行效果
        }
    }

    // 添加属性修饰符的方法，将装备的属性加到玩家属性上
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>(); // 获取玩家的属性组件
        playerStats.strength.AddModifier(strength); // 添加力量修饰符
        playerStats.agility.AddModifier(agility); // 添加敏捷修饰符
        playerStats.intelligence.AddModifier(intelligence); // 添加智力修饰符
        playerStats.vitality.AddModifier(vitality); // 添加体力修饰符

        playerStats.damage.AddModifier(damage); // 添加伤害修饰符
        playerStats.critChance.AddModifier(critChance); // 添加暴击几率修饰符
        playerStats.critPower.AddModifier(critPower); // 添加暴击伤害修饰符

        playerStats.maxHealth.AddModifier(maxHealth); // 添加最大生命值修饰符
        playerStats.armor.AddModifier(armor); // 添加护甲修饰符
        playerStats.evasion.AddModifier(evasion); // 添加闪避修饰符
        playerStats.magicResistance.AddModifier(magicResistance); // 添加魔法抗性修饰符

        playerStats.fireDamage.AddModifier(fireDamage); // 添加火焰伤害修饰符
        playerStats.iceDamage.AddModifier(iceDamage); // 添加冰霜伤害修饰符
        playerStats.lightingDamage.AddModifier(lightingDamage); // 添加闪电伤害修饰符
    }

    // 移除属性修饰符的方法，将装备的属性从玩家属性上移除
    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>(); // 获取玩家的属性组件
        playerStats.strength.RemoveModifier(strength); // 移除力量修饰符
        playerStats.agility.RemoveModifier(agility); // 移除敏捷修饰符
        playerStats.intelligence.RemoveModifier(intelligence); // 移除智力修饰符
        playerStats.vitality.RemoveModifier(vitality); // 移除体力修饰符

        playerStats.damage.RemoveModifier(damage); // 移除伤害修饰符
        playerStats.critChance.RemoveModifier(critChance); // 移除暴击几率修饰符
        playerStats.critPower.RemoveModifier(critPower); // 移除暴击伤害修饰符

        playerStats.maxHealth.RemoveModifier(maxHealth); // 移除最大生命值修饰符
        playerStats.armor.RemoveModifier(armor); // 移除护甲修饰符
        playerStats.evasion.RemoveModifier(evasion); // 移除闪避修饰符
        playerStats.magicResistance.RemoveModifier(magicResistance); // 移除魔法抗性修饰符

        playerStats.fireDamage.RemoveModifier(fireDamage); // 移除火焰伤害修饰符
        playerStats.iceDamage.RemoveModifier(iceDamage); // 移除冰霜伤害修饰符
        playerStats.lightingDamage.RemoveModifier(lightingDamage); // 移除闪电伤害修饰符
    }

    // 获取物品描述的方法，返回物品的详细描述信息
    public override string GetDescription()
    {
        sb.Length = 0; // 清空字符串构建器
        descriptionLength = 0; // 重置描述长度
        AddItemDescription(strength, "strength"); // 添加力量描述
        AddItemDescription(agility, "agility"); // 添加敏捷描述
        AddItemDescription(intelligence, "intelligence"); // 添加智力描述
        AddItemDescription(vitality, "vitality"); // 添加体力描述

        AddItemDescription(damage, "damage"); // 添加伤害描述
        AddItemDescription(critChance, "critChance"); // 添加暴击几率描述
        AddItemDescription(critPower, "critPower"); // 添加暴击伤害描述

        AddItemDescription(maxHealth, "maxHealth"); // 添加最大生命值描述
        AddItemDescription(armor, "armor"); // 添加护甲描述
        AddItemDescription(evasion, "evasion"); // 添加闪避描述
        AddItemDescription(magicResistance, "magicResist"); // 添加魔法抗性描述

        AddItemDescription(fireDamage, "fireDamage"); // 添加火焰伤害描述
        AddItemDescription(iceDamage, "iceDamage"); // 添加冰霜伤害描述
        AddItemDescription(lightingDamage, "lightingDamage"); // 添加闪电伤害描述

        for (int i = 0; i < itemEffects.Length; i++) // 遍历每个物品效果
        {
            if (itemEffects[i].effectDescription.Length > 0) // 如果效果描述不为空
            {
                sb.AppendLine(); // 添加换行
                sb.AppendLine("Unique: " + itemEffects[i].effectDescription); // 添加效果描述
                descriptionLength++; // 增加描述长度
            }
        }

        // 确保描述最小行数为5
        for (int i = 0; i < 5 - descriptionLength; ++i)
        {
            sb.AppendLine(); // 添加换行
            sb.Append(""); // 添加空行
        }

        return sb.ToString(); // 返回完整的描述字符串
    }

    // 添加物品描述的方法，根据属性值和名称生成描述信息
    private void AddItemDescription(int _value, string _name)
    {
        if (_value != 0) // 如果属性值不为0
        {
            if (sb.Length > 0) // 如果字符串构建器不为空
            {
                sb.AppendLine(); // 添加换行
            }
            if (_value > 0) // 如果属性值大于0
            {
                sb.Append("+ " + _value + " " + _name); // 添加正值描述
            }
            descriptionLength++; // 增加描述长度
        }
    }
}
