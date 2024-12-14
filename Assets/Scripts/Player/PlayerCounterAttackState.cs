using UnityEngine; // 引入Unity引擎命名空间

// 定义PlayerCounterAttackState类，继承自PlayerState类，用于处理玩家的反击状态
public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone; // 定义一个布尔变量，用于判断是否可以创建分身

    // 构造函数，用于初始化PlayerCounterAttackState类的实例
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    // Enter方法，进入该状态时调用，初始化状态相关的逻辑
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法，执行进入状态的通用逻辑

        canCreateClone = true; // 初始化为可以创建分身
        stateTimer = player.counterAttackDuration; // 将状态计时器设置为反击持续时间
        player.anim.SetBool("SuccessfulCounterAttack", false); // 设置动画布尔值，表示反击尚未成功
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

        player.SetZeroVelocity(); // 将玩家速度设置为零，防止移动
        // 使用Physics2D.OverlapCircleAll方法检测攻击范围内的所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        // 遍历所有检测到的碰撞体
        foreach (var hit in colliders)
        {
            // 检查碰撞体是否为敌人
            if (hit.GetComponent<Enemy>() != null)
            {
                // 检查敌人是否可以被眩晕
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10; // 将状态计时器设置为大于1的任意值
                    player.anim.SetBool("SuccessfulCounterAttack", true); // 设置动画布尔值，表示反击成功
                    player.skill.parry.UseSkill(); // 使用招架技能

                    // 如果可以创建分身
                    if (canCreateClone)
                    {
                        // 在敌人位置创建分身
                        player.skill.parry.MakeMirageOnParry(hit.transform);
                        canCreateClone = false; // 设置为不可再创建分身
                    }
                }
            }
        }

        // 如果状态计时器小于0或触发器被调用
        if (stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState); // 切换到玩家的空闲状态
        }
    }
}
