using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using TMPro; // 引入TextMeshPro命名空间，用于处理UI文本
using UnityEngine; // 引入Unity引擎命名空间

// UI_StatToolTip类继承自UI_ToolTip，用于显示角色属性信息的工具提示
public class UI_StatToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI description; // 序列化字段，用于显示属性描述的文本组件

    // ShowToolTip方法用于显示属性信息的工具提示
    public void ShowToolTip(string _text)
    {
        description.text = _text; // 设置属性描述文本
        AdjustPosition(); // 调整工具提示的位置

        gameObject.SetActive(true); // 激活工具提示对象
    }

    // HideToolTip方法用于隐藏属性信息的工具提示
    public void HideToolTip()
    {
        description.text = ""; // 清空属性描述文本
        gameObject.SetActive(false); // 隐藏工具提示对象
    }
}
