using UnityEngine; // 引入Unity引擎命名空间

// 定义一个基础Skill类，所有技能的通用基类
public class Skill : MonoBehaviour
{
    // 技能冷却时间，用于存储技能的冷却时间（秒）
    public float cooldown;
    // 技能冷却计时器，用于判断是否处于冷却状态
    public float cooldownTimer;

    // 玩家对象，用于访问玩家的属性和方法
    protected Player player;

    // Start方法在对象被创建时调用，用于初始化玩家对象
    protected virtual void Start()
    {
        player = PlayerManager.instance.player; // 从PlayerManager中获取玩家对象
        CheckUnlock(); // 检查技能是否解锁
    }

    // Update方法每帧调用一次，用于更新技能的冷却时间
    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime; // 每帧减少冷却时间
    }

    // 检查技能解锁状态的方法，子类可以重写此方法
    protected virtual void CheckUnlock()
    {

    }

    // CanUseSkill方法用于检测技能是否可以使用，防止技能在冷却时使用
    public virtual bool CanUseSkill()
    {
        if (cooldownTimer <= 0) // 如果冷却计时器小于等于0，表示可以使用技能
        {
            UseSkill(); // 调用UseSkill方法来使用技能
            cooldownTimer = cooldown; // 重置冷却计时器
            return true; // 返回true表示技能可以使用
        }
        player.fx.CreatePopUpText("Cooldown"); // 如果技能处于冷却状态，显示提示信息
        return false; // 返回false表示技能不能使用
    }

    // UseSkill方法是一个虚方法，子类需要实现具体的技能效果
    public virtual void UseSkill()
    {
        // 子类实现具体的技能效果
    }

    // FindClosestEnemy方法用于查找离指定位置最近的敌人
    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25); // 获取一定范围内的所有碰撞体

        float closestDistance = Mathf.Infinity; // 初始化最近距离为无穷大
        Transform closestEnemy = null; // 初始化最近敌人为空

        foreach (var hit in colliders) // 遍历所有碰撞体
        {
            if (hit.GetComponent<Enemy>() != null) // 如果碰撞体是敌人
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position); // 计算到敌人的距离
                if (distanceToEnemy < closestDistance) // 如果距离小于当前最近距离
                {
                    closestEnemy = hit.transform; // 更新最近敌人
                    closestDistance = distanceToEnemy; // 更新最近距离
                }
            }
        }

        return closestEnemy; // 返回最近的敌人
    }
}