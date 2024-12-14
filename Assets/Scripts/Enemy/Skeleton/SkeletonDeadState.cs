using System.Collections; // 引入System.Collections命名空间
using System.Collections.Generic; // 引入System.Collections.Generic命名空间
using UnityEngine; // 引入UnityEngine命名空间

// SkeletonDeadState类：继承自EnemyState，用于管理骷髅敌人的死亡状态
public class SkeletonDeadState : EnemyState
{
    private Enemy_Skeleton enemy; // 引用Enemy_Skeleton对象，用于访问骷髅敌人的属性和方法

    // 构造函数：初始化骷髅死亡状态
    // <param name="_enemyBase">敌人基类对象</param>
    // <param name="_stateMachin">状态机对象</param>
    // <param name="_animBoolName">动画布尔参数名称</param>
    // <param name="_enemy">骷髅敌人对象</param>
    public SkeletonDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachin, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachin, _animBoolName)
    {
        enemy = _enemy; // 将传入的骷髅敌人对象赋值给成员变量
    }

    // Enter方法：在进入死亡状态时调用
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法
        enemy.anim.SetBool(enemy.lastAnimBoolName, true); // 设置动画布尔参数为true，播放死亡动画
        enemy.anim.speed = 0; // 将动画速度设置为0，停止动画播放
        enemy.cd.enabled = false; // 禁用碰撞检测

        stateTimer = .1f; // 初始化状态计时器
    }

    // Exit方法：在退出死亡状态时调用
    public override void Exit()
    {
        base.Exit(); // 调用基类的Exit方法
    }

    // Update方法：每帧调用一次，用于更新死亡状态
    public override void Update()
    {
        base.Update(); // 调用基类的Update方法

        if(stateTimer > 0) // 如果状态计时器大于0
        {
            rb.velocity = new Vector2(0, 0); // 将刚体速度设置为零，防止移动
        }
    }
}
