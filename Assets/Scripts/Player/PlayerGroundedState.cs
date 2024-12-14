// 引入系统集合命名空间
using System.Collections;
// 引入系统泛型集合命名空间
using System.Collections.Generic;
// 引入Unity引擎命名空间
using UnityEngine;

// 定义PlayerGroundedState类，继承自PlayerState类，用于处理玩家在地面上的状态
public class PlayerGroundedState : PlayerState
{
    // 构造函数，用于初始化PlayerGroundedState类的实例
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        // 检查是否按下R键并且黑洞技能已解锁
        if (Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.blackholeUnlocked)
        {
            // 如果黑洞技能的冷却时间大于0，则显示冷却提示并返回
            if (player.skill.blackhole.cooldownTimer > 0)
            {
                player.fx.CreatePopUpText("Cooldown"); // 显示冷却提示
                return; // 退出方法
            }
            // 切换到黑洞状态
            stateMachine.ChangeState(player.blackholeState);
        }

        // 检查是否按下鼠标右键、没有剑并且剑技能已解锁
        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlocked) { 
            // 切换到瞄准剑状态
            stateMachine.ChangeState(player.aimSword);
        }

        // 检查是否按下Q键并且招架技能已解锁
        if (Input.GetKeyDown(KeyCode.Q) && player.skill.parry.parryUnlocked)
        {
            // 切换到反击状态
            stateMachine.ChangeState(player.counterAttack);
        }

        // 检查是否按下鼠标左键
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // 切换到主要攻击状态
            stateMachine.ChangeState(player.primaryAttack);
        }

        // 检查是否未检测到地面
        if (!player.IsGroundDetected())
        {
            // 切换到空中状态
            stateMachine.ChangeState(player.airState);
        }

        // 检查是否按下空格键并且检测到地面
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            // 切换到跳跃状态
            stateMachine.ChangeState(player.jumpState);
        }
    }

    // HasNoSword方法，用于检查玩家是否没有剑
    private bool HasNoSword()
    {
        // 如果玩家没有剑，返回true
        if (!player.sword)
        {
            return true;
        }

        // 否则，调用剑的ReturnSword方法并返回false
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }

}
