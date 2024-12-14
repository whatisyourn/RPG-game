using UnityEngine; // 引入Unity引擎命名空间

// SkeletonStunnedState类：继承自EnemyState，用于管理骷髅敌人的眩晕状态
public class SkeletonStunnedState : EnemyState
{
    private Enemy_Skeleton enemy; // 引用Enemy_Skeleton对象，用于访问骷髅敌人的属性和方法

    // 构造函数：初始化骷髅眩晕状态
    // <param name="_enemyBase">敌人基类对象</param>
    // <param name="_stateMachin">状态机对象</param>
    // <param name="_animBoolName">动画布尔参数名称</param>
    // <param name="_enemy">骷髅敌人对象</param>
    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachin, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachin, _animBoolName)
    {
        this.enemy = _enemy; // 将传入的骷髅敌人对象赋值给成员变量
    }

    // Enter方法：在进入眩晕状态时调用
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法
        // 使用InvokeRepeating方法每0.1秒调用一次RedColorBlink方法，模拟红色闪烁效果
        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f); // 启动红色闪烁效果

        stateTimer = enemy.stunDuration; // 将状态计时器设置为敌人的眩晕持续时间
        rb.velocity = new Vector2(enemy.stunDirction.x * -enemy.facingDir, enemy.stunDirction.y); // 设置眩晕时的移动方向和速度
    }

    // Exit方法：在退出眩晕状态时调用
    public override void Exit()
    {
        base.Exit(); // 调用基类的Exit方法
        enemy.fx.Invoke("CancelColorChange", 0); // 取消红色闪烁效果
    }

    // Update方法：每帧调用一次，用于更新眩晕状态
    public override void Update()
    {
        base.Update(); // 调用基类的Update方法
        if (stateTimer < 0) // 如果状态计时器小于0
        {
            stateMachine.ChangeState(enemy.idleState); // 改变状态为空闲状态
        }
    }
}