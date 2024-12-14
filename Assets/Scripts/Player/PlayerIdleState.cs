// 引入系统集合命名空间
using System.Collections;
// 引入系统泛型集合命名空间
using System.Collections.Generic;
// 引入Unity引擎命名空间
using UnityEngine;

// 定义PlayerIdleState类，继承自PlayerGroundedState类，用于处理玩家的空闲状态
public class PlayerIdleState : PlayerGroundedState
{
    // 构造函数，用于初始化PlayerIdleState类的实例
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    // Enter方法，进入该状态时调用，初始化状态相关的逻辑
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法，执行进入状态的通用逻辑
        player.SetZeroVelocity(); // 将玩家速度设置为零，防止移动
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

        // 检查玩家的水平输入方向是否与面朝方向一致，并且是否检测到墙壁
        // 如果玩家面朝墙壁且输入方向与面朝方向一致，则不执行任何操作，保持当前状态
        if (xInput == player.facingDir && player.IsWallDetected())
        {
            return; // 直接返回，保持空闲状态
        }

        // 检查玩家是否有水平输入（即是否尝试移动）并且玩家是否不忙碌
        // 如果玩家尝试移动且不忙碌，则切换到移动状态
        if (xInput != 0 && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState); // 切换到玩家的移动状态
        }
    }
}
