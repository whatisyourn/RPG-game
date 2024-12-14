using UnityEngine; // 引入Unity引擎命名空间

// SkeletonBattleState类：继承自EnemyState，用于表示骷髅的战斗状态
public class SkeletonBattleState : EnemyState
{
    // 玩家Transform组件，用于获取玩家的位置
    private Transform player;
    // 骷髅敌人对象，用于访问敌人的属性和方法
    private Enemy_Skeleton enemy;
    // 移动方向变量，1表示向右移动，-1表示向左移动
    private int moveDir;

    // 构造函数：初始化骷髅的战斗状态
    // <param name="_enemyBase">敌人基类对象</param>
    // <param name="_stateMachin">状态机对象</param>
    // <param name="_animBoolName">动画布尔参数名称</param>
    // <param name="_enemy">骷髅敌人对象</param>
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachin, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachin, _animBoolName)
    {
        // 将传入的Enemy_Skeleton对象赋值给enemy成员变量
        this.enemy = _enemy;
    }

    // Enter方法：进入战斗状态时调用
    public override void Enter()
    {
        base.Enter(); // 调用父类的Enter方法
        // 获取PlayerManager中的玩家Transform组件
        player = PlayerManager.instance.player.transform;
        // 如果玩家已经死亡，切换状态为移动状态
        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    // Exit方法：退出战斗状态时调用
    public override void Exit()
    {
        base.Exit(); // 调用父类的Exit方法
    }

    // Update方法：每帧调用一次，用于更新战斗状态
    public override void Update()
    {
        base.Update(); // 调用父类的Update方法
        // 检查是否检测到玩家
        if (enemy.IsPlayerDetected())
        {
            // 重置状态计时器为战斗时间
            stateTimer = enemy.battleTime;
            // 如果检测到玩家的距离小于攻击距离
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                // 如果可以攻击，则切换状态为攻击状态
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            // 如果未检测到玩家，且玩家距离超过一定值，切换状态为空闲状态
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 15)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        // 根据玩家位置确定骷髅的移动方向，并进行相应的移动逻辑
        if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1; // 向右移动
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1; // 向左移动
        }

        // 设置骷髅的速度，包括水平速度和垂直速度
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    // CanAttack方法：用于判断是否可以攻击
    private bool CanAttack()
    {
        // 如果当前时间大于等于上次攻击时间加上攻击冷却时间，则可以攻击
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            // 随机生成新的攻击冷却时间
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            // 更新上次攻击时间为当前时间
            enemy.lastTimeAttacked = Time.time;
            return true; // 返回true表示可以攻击
        }
        // 否则返回false表示不能攻击
        return false;
    }
}