using UnityEngine; // 引入Unity引擎命名空间

// 定义一个抽象的物品效果类，继承自ScriptableObject
public class ItemEffect : ScriptableObject
{
    // 使用TextArea属性使effectDescription在Unity编辑器中显示为多行文本框
    [TextArea]
    // 定义一个公共字符串变量，用于描述效果
    public string effectDescription;

    // 定义一个虚方法，允许子类重写，执行效果
    public virtual void ExecuteEffect(Transform _enemyPosition)
    {
        // 输出调试信息，表示效果已执行
        Debug.Log("Effect executed");
    }
}
