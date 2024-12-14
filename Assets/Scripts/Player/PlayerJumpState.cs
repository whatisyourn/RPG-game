using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// 定义PlayerJumpState类，继承自PlayerState类，用于处理玩家的跳跃状态
public class PlayerJumpState : PlayerState
{
    // 构造函数，用于初始化PlayerJumpState类的实例
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    // Enter方法，进入该状态时调用，初始化跳跃状态相关的逻辑
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法，执行进入状态的通用逻辑
        // 设置玩家的水平速度和垂直速度，实现跳跃效果
        player.SetVelocity(rb.velocity.x, player.jumpForce);
    }

    // Exit方法，退出该状态时调用，处理状态退出时的逻辑
    public override void Exit()
    {
        base.Exit(); // 调用基类的Exit方法，执行退出状态的通用逻辑
    }

    // Update方法，每帧调用一次，用于更新跳跃状态的逻辑
    public override void Update()
    {
        base.Update(); // 调用基类的Update方法，执行每帧更新的通用逻辑
        // 检查玩家是否开始下落（即速度的y分量小于0）
        if (rb.velocity.y < 0)
        {
            // 如果开始下落，则切换到空中状态
            stateMachine.ChangeState(player.airState);
        }
    }
}