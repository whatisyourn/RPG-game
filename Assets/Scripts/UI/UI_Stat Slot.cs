using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using TMPro; // 引入TextMeshPro命名空间，用于处理UI文本
using UnityEngine; // 引入Unity引擎命名空间
using UnityEngine.EventSystems; // 引入事件系统命名空间，用于处理UI事件

// UI_StatSlot类用于管理和显示角色属性的UI槽
public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    private UI ui; // UI类的引用，用于访问UI组件
    [SerializeField] private string statName; // 序列化字段，存储属性名称
    [SerializeField] private StatType statType; // 序列化字段，存储属性类型
    [SerializeField] private TextMeshProUGUI statValueText; // 序列化字段，用于显示属性值的文本组件
    [SerializeField] private TextMeshProUGUI statNameText; // 序列化字段，用于显示属性名称的文本组件

    [TextArea]
    [SerializeField] private string statDescription; // 序列化字段，存储属性描述

    // OnValidate方法在编辑器中更改值时调用，用于更新游戏对象名称和属性名称文本
    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName; // 设置游戏对象名称为"Stat - "加上属性名称

        if (statNameText != null) // 如果属性名称文本组件不为空
        {
            statNameText.text = statName; // 更新属性名称文本
        }
    }

    // Start方法在游戏开始时调用，用于初始化属性槽
    void Start()
    {
        UpdateStatValueUI(); // 更新属性值的UI显示
        ui = GetComponentInParent<UI>(); // 获取父对象中的UI组件
    }

    // UpdateStatValueUI方法用于更新属性值的UI显示
    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>(); // 获取玩家的属性组件

        if (playerStats != null) { // 如果玩家属性组件不为空
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString(); // 获取并显示对应属性的值

            if(statType == StatType.health) // 如果属性类型是健康值
            {
                statValueText.text = playerStats.GetMaxHealthValue().ToString(); // 显示最大健康值
            }

            if(statType == StatType.damage) // 如果属性类型是伤害
            {
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString(); // 显示伤害加力量的总值
            }
            if (statType == StatType.critPower) // 如果属性类型是暴击力量
            {
                statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString(); // 显示暴击力量加力量的总值
            }
            if (statType == StatType.critChance) // 如果属性类型是暴击几率
            {
                statValueText.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString(); // 显示暴击几率加敏捷的总值
            }
            if (statType == StatType.evasion) // 如果属性类型是闪避
            {
                statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString(); // 显示闪避加敏捷的总值
            }
            if (statType == StatType.magicResistance) // 如果属性类型是魔法抗性
            {
                statValueText.text = (playerStats.magicResistance.GetValue() + playerStats.intelligence.GetValue() * 3).ToString(); // 显示魔法抗性加智力乘以3的总值
            }
        }
    }

    // OnPointerEnter方法在鼠标指针进入UI元素时调用，用于显示属性提示
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(statDescription); // 显示属性描述的工具提示
    }

    // OnPointerExit方法在鼠标指针离开UI元素时调用，用于隐藏属性提示
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.HideToolTip(); // 隐藏工具提示
    }
}
