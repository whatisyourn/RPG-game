using UnityEngine; // 引入Unity引擎命名空间

// 使用CreateAssetMenu属性创建一个可在Unity编辑器中创建的Buff效果脚本对象
[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item effect/Buff effect")]
public class Buff_Effect : ItemEffect
{
    // 定义一个PlayerStats类型的私有变量，用于存储玩家的状态
    private PlayerStats stats;
    // 使用SerializeField属性使私有变量在Unity编辑器中可见，定义buff的类型
    [SerializeField] private StatType buffType;
    // 使用SerializeField属性使私有变量在Unity编辑器中可见，定义buff的数值
    [SerializeField] private int buffAmount;
    // 使用SerializeField属性使私有变量在Unity编辑器中可见，定义buff的持续时间
    [SerializeField] private float buffDuration;

    // 重写基类的ExecuteEffect方法，执行buff效果
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        // 获取玩家的PlayerStats组件
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        // 调用PlayerStats组件的IncreaseStatBy方法，增加玩家的指定属性
        stats.IncreaseStatBy(buffAmount, buffDuration, stats.GetStat(buffType));
    }
}
