using System.Collections; // 引入System.Collections命名空间
using System.Collections.Generic; // 引入System.Collections.Generic命名空间
using UnityEngine; // 引入UnityEngine命名空间

// SkeletonMoveState类：继承自SkeletonGroundedState，用于管理骷髅敌人的移动状态
public class SkeletonMoveState : SkeletonGroundedState
{
    // 构造函数：初始化骷髅移动状态
    // <param name="_enemyBase">敌人基类对象</param>
    // <param name="_stateMachin">状态机对象</param>
    // <param name="_animBoolName">动画布尔参数名称</param>
    // <param name="_enemy">骷髅敌人对象</param>
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachin, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachin, _animBoolName, _enemy)
    {
    }

    // Enter方法：在进入移动状态时调用
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法
    }

    // Exit方法：在退出移动状态时调用
    public override void Exit()
    {
        base.Exit(); // 调用基类的Exit方法
    }

    // Update方法：每帧调用一次，用于更新移动状态
    public override void Update()
    {
        base.Update(); // 调用基类的Update方法
        // 使用敌人的速度进行移动
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rb.velocity.y);
        // 如果检测到墙壁或前方没有地面，则翻转方向并切换到空闲状态
        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip(); // 翻转敌人的朝向
            stateMachine.ChangeState(enemy.idleState); // 切换状态为空闲状态
        }
    }
}
