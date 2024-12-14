using UnityEngine; // 引入Unity引擎命名空间

// 使用CreateAssetMenu属性创建一个可在Unity编辑器中创建的冻结敌人效果脚本对象
[CreateAssetMenu(fileName = "Freeze enemies effect", menuName = "Data/Item effect/Freeze Enemies")]
public class FreezeEnemies_Effect : ItemEffect
{
    // 使用SerializeField属性使私有变量在Unity编辑器中可见，定义冻结效果的持续时间
    [SerializeField] private float duration;

    // 重写基类的ExecuteEffect方法，执行冻结敌人效果
    public override void ExecuteEffect(Transform _transfor)
    {
        // 获取玩家的PlayerStats组件
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        // 如果玩家当前生命值大于最大生命值的10%，则返回，不执行效果
        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * .1f)
        {
            return;
        }

        // 如果未能使用护甲，则返回，不执行效果
        if (!Inventory.instance.UseArmor())
        {
            return;
        }
        // 使用Physics2D.OverlapCircleAll方法，以传入的_transform.position为圆心，2为半径，检测范围内的所有Collider2D对象
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transfor.position, 2);

        // 遍历检测到的所有Collider2D对象
        foreach (var hit in colliders)
        {
            // 如果检测到的对象是Enemy类型，则调用其FreezeTimeFor方法，冻结敌人指定的持续时间
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
