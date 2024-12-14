using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// 定义PlayerWallJumpState类，继承自PlayerState类，用于处理玩家的墙壁跳跃状态
public class PlayerWallJumpState : PlayerState
{
    // 构造函数，用于初始化PlayerWallJumpState类的实例
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    // Enter方法，进入该状态时调用，初始化墙壁跳跃状态相关的逻辑
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法，执行进入状态的通用逻辑

        // 设置状态计时器为0.4秒，用于控制墙壁跳跃状态的持续时间
        stateTimer = .4f;

        // 设置玩家的初始速度，包括水平速度和垂直速度
        // 5 * -player.facingDir计算玩家反方向的水平速度，负号表示相反方向
        // player.jumpForce用于给玩家跳跃提供向上的垂直速度
        player.SetVelocity(5 * -player.facingDir, player.jumpForce);
    }

    // Exit方法，退出该状态时调用，处理状态退出时的逻辑
    public override void Exit()
    {
        base.Exit(); // 调用基类的Exit方法，执行退出状态的通用逻辑
    }

    // Update方法，每帧调用一次，用于更新墙壁跳跃状态的逻辑
    public override void Update()
    {
        base.Update(); // 调用基类的Update方法，执行每帧更新的通用逻辑

        // 检查状态计时器是否小于0，如果是则切换到空中状态
        if (stateTimer < 0)
        {
            // 状态计时器小于0时，切换到空中状态
            stateMachine.ChangeState(player.airState);
        }

        // 检查玩家是否检测到地面，如果检测到则切换到空闲状态
        if (player.IsGroundDetected())
        {
            // 如果检测到地面，切换到空闲状态
            stateMachine.ChangeState(player.idleState);
        }
    }
}
