using System.Collections.Generic; // 引入系统集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

[System.Serializable] // 使类可以在Unity编辑器中序列化
public class Stat
{
    [SerializeField] private int baseValue; // 基础值，使用SerializeField使其在Unity编辑器中可见

    public List<int> modifiers; // 修饰符列表，用于存储所有的修饰值

    // 获取属性值的方法，返回基础值加上所有修饰值的总和
    public int GetValue()
    {
        int finalValue = baseValue; // 初始化最终值为基础值
        foreach (int modifier in modifiers) // 遍历所有修饰值
        {
            finalValue += modifier; // 将修饰值加到最终值上
        }
        return finalValue; // 返回最终值
    }

    // 设置基础值的方法
    public void SetDefaultValue(int _value)
    {
        baseValue = _value; // 将传入的值设置为基础值
    }

    // 添加修饰值的方法
    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier); // 将修饰值添加到修饰符列表中
    }

    // 移除修饰值的方法
    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier); // 从修饰符列表中移除修饰值
    }

}
