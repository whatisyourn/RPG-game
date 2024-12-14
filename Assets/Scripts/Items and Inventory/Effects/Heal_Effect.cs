using UnityEngine; // 引入Unity引擎命名空间

// 使用CreateAssetMenu属性创建一个可在Unity编辑器中创建的治疗效果脚本对象
[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item effect/Heal effect")]
public class Heal_Effect : ItemEffect
{
    // 使用Range属性限制healPercent的取值范围在0到1之间
    [Range(0f, 1f)]
    // 使用SerializeField属性使私有变量在Unity编辑器中可见，定义治疗百分比
    [SerializeField] private float healPercent;

    // 重写基类的ExecuteEffect方法，执行治疗效果
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        // 获取玩家的PlayerStats组件
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        // 计算治疗量，将玩家最大生命值乘以治疗百分比并四舍五入
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);
        // 调用PlayerStats组件的IncreaseHealthBy方法，增加玩家的生命值
        playerStats.IncreaseHealthBy(healAmount);
    }
}
