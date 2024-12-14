using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// 定义PlayerBlackholeState类，继承自PlayerState类，用于处理玩家的黑洞状态
public class PlayerBlackholeState : PlayerState
{
    private float flyTime = .4f; // 定义飞行时间为0.4秒
    private bool skillUsed; // 定义一个布尔值用于判断技能是否已使用
    private float defaultGravity; // 定义一个浮点数用于存储默认重力值

    // 构造函数，用于初始化PlayerBlackholeState类的实例
    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
        defaultGravity = rb.gravityScale; // 保存当前的重力比例
        skillUsed = false; // 初始化技能未使用状态
        stateTimer = flyTime; // 将状态计时器设置为飞行时间
        rb.gravityScale = 0; // 将重力比例设置为0，使玩家悬浮
    }

    // Exit方法，退出该状态时调用，处理状态退出时的逻辑
    public override void Exit()
    {
        base.Exit(); // 调用基类的Exit方法
        rb.gravityScale = defaultGravity; // 恢复默认重力比例
        PlayerManager.instance.player.fx.MakeTransparent(false); // 取消玩家透明效果
    }

    // Update方法，每帧调用一次，用于更新状态逻辑
    public override void Update()
    {
        base.Update(); // 调用基类的Update方法

        if (stateTimer > 0) // 如果状态计时器大于0
        {
            rb.velocity = new Vector2(0, 15); // 设置玩家的速度向上移动
        }
        else if (stateTimer < 0) // 如果状态计时器小于0
        {
            rb.velocity = new Vector2(0, -.1f); // 设置玩家的速度向下缓慢移动

            if (!skillUsed) // 如果技能未使用
            {
                if (player.skill.blackhole.CanUseSkill()) // 检查黑洞技能是否可以使用
                    skillUsed = true; // 标记技能已使用
            }
        }

        if (player.skill.blackhole.SkillCompleted()) // 检查黑洞技能是否完成
        {
            stateMachine.ChangeState(player.airState); // 切换到空中状态
        }
    }
}
