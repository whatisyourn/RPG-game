using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using TMPro; // 引入TextMeshPro命名空间，用于UI文本处理
using UnityEngine; // 引入Unity引擎命名空间

// UI_SkillToolTip类继承自UI_ToolTip，用于显示技能信息的工具提示
public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillText; // 序列化字段，用于显示技能描述的文本组件
    [SerializeField] private TextMeshProUGUI skillName; // 序列化字段，用于显示技能名称的文本组件
    [SerializeField] private TextMeshProUGUI skillCost; // 序列化字段，用于显示技能消耗的文本组件
    [SerializeField] private float defaultFontSize; // 序列化字段，默认字体大小

    // ShowToolTip方法用于显示技能信息的工具提示
    public void ShowToolTip(string _skillDescription, string _skillName, int _price)
    {
        skillText.text = _skillDescription; // 设置技能描述文本
        skillName.text = _skillName; // 设置技能名称文本
        skillCost.text = "Cost: " + _price.ToString(); // 设置技能消耗文本
        AdjustPosition(); // 调整工具提示的位置
        AdjustFontSize(skillName); // 调整技能名称文本的字体大小

        gameObject.SetActive(true); // 激活工具提示对象
    }

    // HideToolTip方法用于隐藏技能信息的工具提示
    public void HideToolTip()
    {
        skillName.fontSize = defaultFontSize; // 重置技能名称文本的字体大小为默认值
        gameObject.SetActive(false); // 隐藏工具提示对象
    }
}
