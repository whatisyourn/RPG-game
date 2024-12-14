using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// PlayerAirState类继承自PlayerState类，表示玩家在空中的状态
public class PlayerAirState : PlayerState
{
    // 构造函数，用于初始化玩家空中状态
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        // 通过基类构造函数初始化玩家、状态机和动画布尔值名称
    }

    // Enter方法，进入该状态时调用，初始化状态相关的逻辑
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法，执行进入状态的通用逻辑
    }

    // Exit方法，退出该状态时调用，处理状态退出时的逻辑
    public override void Exit()
    {
        base.Exit(); // 调用基类的Exit方法，执行退出状态的通用逻辑
    }

    // Update方法，每帧调用一次，用于更新状态逻辑
    public override void Update()
    {
        base.Update(); // 调用基类的Update方法，执行每帧更新的通用逻辑

        // 检查是否检测到墙壁，如果检测到则切换到墙壁滑动状态
        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState); // 切换到墙壁滑动状态
        }

        // 检查是否检测到地面，如果检测到则切换到空闲状态
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState); // 切换到空闲状态
        }

        // 检查是否有水平输入，如果有则调整玩家的水平速度
        if (xInput != 0)
        {
            // 设置玩家的水平速度为移动速度的0.8倍乘以输入方向，保持水平移动的平滑性
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);
        }
    }
}