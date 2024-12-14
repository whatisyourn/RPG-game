// 引入系统集合命名空间
using System.Collections;
// 引入系统泛型集合命名空间
using System.Collections.Generic;
// 引入Unity引擎命名空间
using UnityEngine;

// 定义PlayerMoveState类，继承自PlayerGroundedState类，用于处理玩家的移动状态
public class PlayerMoveState : PlayerGroundedState
{
    // 构造函数，用于初始化PlayerMoveState类的实例
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
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

        // 设置玩家的水平速度和垂直速度
        // xInput * player.moveSpeed 计算玩家的水平移动速度，rb.velocity.y 保持垂直速度不变
        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        // 检查水平输入（xInput）是否为0或者是否检测到墙壁
        if (xInput == 0 || player.IsWallDetected())
        {
            // 如果没有水平移动或者检测到墙壁，则切换到空闲状态
            stateMachine.ChangeState(player.idleState);
        }
    }
}
