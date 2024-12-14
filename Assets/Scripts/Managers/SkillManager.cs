using UnityEngine; // 引入Unity引擎命名空间

public class SkillManager : MonoBehaviour // 定义SkillManager类，继承自MonoBehaviour
{
    public static SkillManager instance; // 定义一个静态实例，用于实现单例模式

    #region Skill
    public Dash_Skill dash { get; private set; } // 定义一个Dash_Skill类型的属性，用于存储冲刺技能
    public Clone_Skill clone { get; private set; } // 定义一个Clone_Skill类型的属性，用于存储克隆技能
    public Sword_Skill sword { get; private set; } // 定义一个Sword_Skill类型的属性，用于存储剑术技能
    public Blackhole_Skill blackhole { get; private set; } // 定义一个Blackhole_Skill类型的属性，用于存储黑洞技能
    public Crystal_Skill crystal { get; private set; } // 定义一个Crystal_Skill类型的属性，用于存储水晶技能
    public Parry_Skill parry { get; private set; } // 定义一个Parry_Skill类型的属性，用于存储招架技能
    public Dodge_Skill dodge { get; private set; } // 定义一个Dodge_Skill类型的属性，用于存储闪避技能
    #endregion

    private void Awake() // Awake方法用于初始化单例模式
    {
        // 确保全局只有一个SkillManager实例
        if (instance == null) // 如果实例为空
            instance = this; // 将当前实例赋值给静态实例
        else
            Destroy(instance.gameObject); // 如果实例不为空，销毁已有的实例对象
    }

    private void Start() // Start方法用于在游戏开始时获取技能组件
    {
        // 在游戏开始时获取技能组件
        dash = GetComponent<Dash_Skill>(); // 获取冲刺技能组件
        clone = GetComponent<Clone_Skill>(); // 获取克隆技能组件
        sword = GetComponent<Sword_Skill>(); // 获取剑术技能组件
        blackhole = GetComponent<Blackhole_Skill>(); // 获取黑洞技能组件
        crystal = GetComponent<Crystal_Skill>(); // 获取水晶技能组件
        parry = GetComponent<Parry_Skill>(); // 获取招架技能组件
        dodge = GetComponent<Dodge_Skill>(); // 获取闪避技能组件
    }
}