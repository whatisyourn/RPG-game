using UnityEngine; // 引入Unity引擎命名空间

// SkeletonGroundedState类：继承自EnemyState，用于管理骷髅敌人在地面上的状态
public class SkeletonGroundedState : EnemyState
{
    protected Enemy_Skeleton enemy; // 引用Enemy_Skeleton对象，用于访问骷髅敌人的属性和方法
    protected Transform player; // 引用玩家的Transform组件，用于获取玩家的位置

    // 构造函数：初始化骷髅在地面上的状态
    // <param name="_enemyBase">敌人基类对象</param>
    // <param name="_stateMachin">状态机对象</param>
    // <param name="_animBoolName">动画布尔参数名称</param>
    // <param name="_enemy">骷髅敌人对象</param>
    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachin, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachin, _animBoolName)
    {
        this.enemy = _enemy; // 将传入的骷髅敌人对象赋值给成员变量
    }

    // Enter方法：在进入地面状态时调用
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法
        player = PlayerManager.instance.player.transform; // 获取玩家的Transform组件
    }

    // Exit方法：在退出地面状态时调用
    public override void Exit()
    {
        base.Exit(); // 调用基类的Exit方法
    }

    // Update方法：每帧调用一次，用于更新地面状态
    public override void Update()
    {
        base.Update(); // 调用基类的Update方法
        // 检查是否检测到玩家或玩家距离小于2
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2)
        {
            stateMachine.ChangeState(enemy.battleState); // 如果条件满足，改变状态为战斗状态
        }
    }
}