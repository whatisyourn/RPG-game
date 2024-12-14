using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// 定义PlayerPrimaryAttackState类，继承自PlayerState类，用于处理玩家的主要攻击状态
public class PlayerPrimaryAttackState : PlayerState
{
    // comboCounter属性，用于记录当前的连击次数
    public int comboCounter { get; private set; }
    // lastTimeAttacked变量，记录上一次攻击的时间，用于判断是否在连击窗口内
    private float lastTimeAttacked;
    // comboWindow变量，定义连击的时间窗口，如果超过这个时间未进行攻击，则重置连击
    private float comboWindow = 2;

    // 构造函数，用于初始化PlayerPrimaryAttackState类的实例
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    // Enter方法，进入该状态时调用，初始化攻击状态相关的逻辑
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法，执行进入状态的通用逻辑

        // AudioManager.instance.PlaySFX(2); // 播放攻击音效，注释掉以便在其他地方调用

        xInput = 0; // 重置水平输入，防止潜在的bug

        // 如果连击次数超过2次或超过连击窗口时间，则重置连击计数
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0; // 重置连击计数
        }
        player.anim.SetInteger("ComboCounter", comboCounter); // 设置动画中的连击计数

        // 计算攻击的方向，默认为玩家面朝方向，如果有水平输入则使用输入方向
        float attackDir = player.facingDir;
        if (xInput != 0)
        {
            attackDir = xInput; // 使用输入方向
        }

        // 设置玩家的移动速度，基于当前连击次数的攻击移动参数和攻击方向
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);
        stateTimer = .1f; // 设置状态计时器
    }

    // Exit方法，退出该状态时调用，处理状态退出时的逻辑
    public override void Exit()
    {
        base.Exit(); // 调用基类的Exit方法，执行退出状态的通用逻辑
        player.StartCoroutine("BusyFor", .15f); // 使玩家忙碌0.15秒，表示攻击后的硬直
        comboCounter++; // 增加连击计数
        lastTimeAttacked = Time.time; // 更新上一次攻击的时间
    }

    // Update方法，每帧调用一次，用于更新攻击状态的逻辑
    public override void Update()
    {
        base.Update(); // 调用基类的Update方法，执行每帧更新的通用逻辑
        // 如果状态计时器小于0，则将玩家速度设置为零
        if (stateTimer < 0)
        {
            player.SetZeroVelocity(); // 停止玩家移动
        }

        // 如果触发器被调用，则切换到空闲状态
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState); // 切换到玩家的空闲状态
        }
    }
}