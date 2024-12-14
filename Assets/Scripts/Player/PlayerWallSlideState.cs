using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// 定义PlayerWallSlideState类，继承自PlayerState类，用于处理玩家的墙壁滑动状态
public class PlayerWallSlideState : PlayerState
{
    // 构造函数，用于初始化PlayerWallSlideState类的实例
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    // Enter方法，进入该状态时调用，初始化墙壁滑动状态相关的逻辑
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法，执行进入状态的通用逻辑
    }

    // Exit方法，退出该状态时调用，处理状态退出时的逻辑
    public override void Exit()
    {
        base.Exit(); // 调用基类的Exit方法，执行退出状态的通用逻辑
    }

    // Update方法，每帧调用一次，用于更新墙壁滑动状态的逻辑
    public override void Update()
    {
        base.Update(); // 调用基类的Update方法，执行每帧更新的通用逻辑

        // 检查玩家是否不再检测到墙壁
        if (!player.IsWallDetected()) { 
            // 如果没有检测到墙壁，则切换到空中状态
            stateMachine.ChangeState(player.airState);
        }

        // 检查玩家是否按下了空格键以进行墙壁跳跃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 如果按下空格键，则切换到墙壁跳跃状态
            stateMachine.ChangeState(player.wallJumpState);
            // 退出当前方法，防止后续代码执行，确保状态切换
            return;
        }

        // 检查垂直输入（yInput）是否小于0，即玩家是否在向下移动
        if (yInput < 0)
        {
            // 如果玩家在向下移动，则设置垂直速度为当前速度，保持下降
            player.SetVelocity(0, rb.velocity.y);
        }
        else
        {
            // 如果玩家没有向下移动，则减缓垂直速度，使用一个系数，模拟摩擦力
            player.SetVelocity(0, rb.velocity.y * 0.7f);
        }

        // 检查水平输入（xInput）是否不为0且与玩家当前面朝方向相反，或者是否检测到地面
        if ((xInput != 0 && player.facingDir != xInput) || player.IsGroundDetected())
        {
            // 如果满足条件，则切换到空闲状态
            stateMachine.ChangeState(player.idleState);
        }
    }
}
