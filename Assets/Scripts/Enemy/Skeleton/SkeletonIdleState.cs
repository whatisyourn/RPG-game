// SkeletonIdleState类：继承自SkeletonGroundedState，用于管理骷髅敌人的空闲状态
public class SkeletonIdleState : SkeletonGroundedState
{
    // 构造函数：初始化骷髅空闲状态
    // <param name="_enemyBase">敌人基类对象</param>
    // <param name="_stateMachin">状态机对象</param>
    // <param name="_animBoolName">动画布尔参数名称</param>
    // <param name="_enemy">骷髅敌人对象</param>
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachin, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachin, _animBoolName, _enemy)
    {
        // 构造函数体为空，因为初始化工作已在基类中完成
    }

    // Enter方法：在进入空闲状态时调用
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法
        stateTimer = enemy.idleTime; // 将状态计时器设置为敌人的空闲时间
    }

    // Exit方法：在退出空闲状态时调用
    public override void Exit()
    {
        base.Exit(); // 调用基类的Exit方法
        AudioManager.instance.PlaySFX(1, enemy.transform); // 播放音效，参数1表示音效ID
    }

    // Update方法：每帧调用一次，用于更新空闲状态
    public override void Update()
    {
        base.Update(); // 调用基类的Update方法
        // 如果状态计时器小于0，表示空闲时间结束，切换到移动状态
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState); // 改变状态为移动状态
        }
    }
}
