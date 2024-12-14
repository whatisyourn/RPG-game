using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// 定义PlayerDeadState类，继承自PlayerState类，用于处理玩家死亡状态
public class PlayerDeadState : PlayerState
{
    // 构造函数，用于初始化PlayerDeadState类的实例
    public PlayerDeadState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    // AnimationFinishTrigger方法，用于在动画完成时触发
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger(); // 调用基类的AnimationFinishTrigger方法
    }

    // Enter方法，进入该状态时调用，初始化状态相关的逻辑
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法
        GameObject.Find("Canvas").GetComponent<UI>().SwitchOnEndScreen(); // 切换到结束屏幕
    }

    // Exit方法，退出该状态时调用，处理状态退出时的逻辑
    public override void Exit()
    {
        base.Exit(); // 调用基类的Exit方法
    }

    // Update方法，每帧调用一次，用于更新状态逻辑
    public override void Update()
    {
        base.Update(); // 调用基类的Update方法
        player.SetZeroVelocity(); // 将玩家速度设置为零，防止移动
    }
}
