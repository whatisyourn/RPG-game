using UnityEngine; // 引入Unity引擎命名空间

// 定义PlayerDashState类，继承自PlayerState类，用于处理玩家的冲刺状态
public class PlayerDashState : PlayerState
{
    // 构造函数，用于初始化PlayerDashState类的实例
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    // Enter方法，进入该状态时调用，初始化状态相关的逻辑
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法，执行进入状态的通用逻辑

        player.skill.dash.CloneOnDash(); // 在冲刺时创建分身

        player.stats.MakeInvincible(true); // 使玩家在冲刺时无敌

        // 设置状态计时器为玩家的冲刺持续时间
        stateTimer = player.dashDuration;
    }

    // Exit方法，退出该状态时调用，处理状态退出时的逻辑
    public override void Exit()
    {
        base.Exit(); // 调用基类的Exit方法，执行退出状态的通用逻辑
        player.stats.MakeInvincible(false); // 取消玩家的无敌状态
        player.skill.dash.CloneOnArrival(); // 在到达时创建分身

        // 在退出冲刺状态时，将玩家的水平速度设置为0，保持垂直速度不变
        player.SetVelocity(0, rb.velocity.y);
    }

    // Update方法，每帧调用一次，用于更新状态逻辑
    public override void Update()
    {
        base.Update(); // 调用基类的Update方法，执行每帧更新的通用逻辑

        // 如果没有检测到地面但检测到墙壁，则切换到墙壁滑动状态
        if (!player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState); // 切换到墙壁滑动状态
        }

        // 设置玩家的速度为冲刺速度乘以冲刺方向，实现冲刺效果
        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        // 如果状态计时器小于0，则切换到空闲状态
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState); // 切换到空闲状态
        }

        player.fx.CreateAfterImage(); // 创建冲刺后的残影效果
    }
}
