using UnityEngine; // 引入Unity引擎命名空间

// SkeletonAttackState类：继承自EnemyState，用于管理骷髅敌人的攻击状态
public class SkeletonAttackState : EnemyState
{
    private Enemy_Skeleton enemy; // 引用Enemy_Skeleton对象，用于访问骷髅敌人的属性和方法

    // 构造函数：初始化骷髅攻击状态
    // <param name="_enemyBase">敌人基类对象</param>
    // <param name="_stateMachin">状态机对象</param>
    // <param name="_animBoolName">动画布尔参数名称</param>
    // <param name="_enemy">骷髅敌人对象</param>
    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachin, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachin, _animBoolName)
    {
        this.enemy = _enemy; // 将传入的骷髅敌人对象赋值给成员变量
    }

    // Enter方法：在进入攻击状态时调用
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法
        // 此处可以添加进入攻击状态时的逻辑，例如播放攻击动画
    }

    // Exit方法：在退出攻击状态时调用
    public override void Exit()
    {
        base.Exit(); // 调用基类的Exit方法
        enemy.lastTimeAttacked = Time.time; // 记录最后一次攻击的时间
    }

    // Update方法：每帧调用一次，用于更新攻击状态
    public override void Update()
    {
        base.Update(); // 调用基类的Update方法
        enemy.SetZeroVelocity(); // 设置骷髅敌人的速度为零，防止移动
        if (triggerCalled) // 如果攻击动画触发器被调用
        {
            stateMachine.ChangeState(enemy.battleState); // 改变状态为战斗状态
        }
    }
}