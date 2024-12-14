using System.Collections;
using UnityEngine;

// 定义Player类，继承自Entity类
public class Player : Entity
{
    [Header("Attack details")] 
    public Vector2[] attackMovement; // 定义一个向量数组，用于存储攻击移动的方向和距离
    public float counterAttackDuration = .2f; // 定义反击的持续时间

    public bool isBusy { get; private set; } // 定义一个只读属性，表示玩家是否正忙

    [Header("Move info")] 
    public float moveSpeed = 12f; // 定义玩家的移动速度
    public float jumpForce; // 定义玩家的跳跃力
    public float swordReturnImpact; // 定义剑返回时的冲击力
    private float defaultMoveSpeed; // 存储默认的移动速度
    private float defaultJumpForce; // 存储默认的跳跃力

    [Header("Dash info")] 
    public float dashSpeed; // 定义冲刺速度
    public float dashDuration; // 定义冲刺持续时间
    private float defaultDashSpeed; // 存储默认的冲刺速度
    public float dashDir { get; private set; } // 定义一个只读属性，表示冲刺方向

    public SkillManager skill { get; private set; } // 定义一个只读属性，引用技能管理器
    public GameObject sword { get; private set; } // 定义一个只读属性，引用玩家的剑对象
    public PlayerFX fx { get; private set; } // 定义一个只读属性，引用玩家的特效管理器
    #region State
    public PlayerStateMachine stateMachine { get; private set; } // 定义一个只读属性，引用玩家状态机
    public PlayerIdleState idleState { get; private set; } // 定义一个只读属性，引用玩家的空闲状态
    public PlayerMoveState moveState { get; private set; } // 定义一个只读属性，引用玩家的移动状态
    public PlayerJumpState jumpState { get; private set; } // 定义一个只读属性，引用玩家的跳跃状态
    public PlayerAirState airState { get; private set; } // 定义一个只读属性，引用玩家的空中状态
    public PlayerWallSlideState wallSlideState { get; private set; } // 定义一个只读属性，引用玩家的墙滑状态
    public PlayerDashState dashState { get; private set; } // 定义一个只读属性，引用玩家的冲刺状态
    public PlayerWallJumpState wallJumpState { get; private set; } // 定义一个只读属性，引用玩家的墙跳状态
    public PlayerPrimaryAttackState primaryAttack { get; private set; } // 定义一个只读属性，引用玩家的主要攻击状态
    public PlayerCounterAttackState counterAttack { get; private set; } // 定义一个只读属性，引用玩家的反击状态
    public PlayerCatchSwordState catchSword { get; private set; } // 定义一个只读属性，引用玩家的接剑状态
    public PlayerAimSwordState aimSword { get; private set; } // 定义一个只读属性，引用玩家的瞄准剑状态
    public PlayerBlackholeState blackholeState { get; private set; } // 定义一个只读属性，引用玩家的黑洞状态
    public PlayerDeadState deadState { get; private set; } // 定义一个只读属性，引用玩家的死亡状态
    #endregion

    protected override void Awake()
    {
        base.Awake(); // 调用基类的Awake方法
        stateMachine = new PlayerStateMachine(); // 初始化玩家状态机

        idleState = new PlayerIdleState(this, stateMachine, "Idle"); // 初始化空闲状态
        moveState = new PlayerMoveState(this, stateMachine, "Move"); // 初始化移动状态
        jumpState = new PlayerJumpState(this, stateMachine, "Jump"); // 初始化跳跃状态
        airState = new PlayerAirState(this, stateMachine, "Jump"); // 初始化空中状态
        dashState = new PlayerDashState(this, stateMachine, "Dash"); // 初始化冲刺状态
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide"); // 初始化墙滑状态
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump"); // 初始化墙跳状态

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack"); // 初始化主要攻击状态
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack"); // 初始化反击状态

        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword"); // 初始化瞄准剑状态
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword"); // 初始化接剑状态
        blackholeState = new PlayerBlackholeState(this, stateMachine, "Jump"); // 初始化黑洞状态
        deadState = new PlayerDeadState(this, stateMachine, "Dead"); // 初始化死亡状态
    }

    protected override void Start()
    {
        base.Start(); // 调用基类的Start方法
        fx = GetComponent<PlayerFX>(); // 获取玩家特效组件
        skill = SkillManager.instance; // 获取技能管理器实例

        stateMachine.Initialize(idleState); // 初始化状态机为空闲状态

        defaultMoveSpeed = moveSpeed; // 存储默认移动速度
        defaultJumpForce = jumpForce; // 存储默认跳跃力
        defaultDashSpeed = dashSpeed; // 存储默认冲刺速度
    }

    protected override void Update()
    {
        if(Time.timeScale == 0) // 检查游戏是否暂停
        {
            return; // 如果暂停则返回
        }

        base.Update(); // 调用基类的Update方法
        stateMachine.currenState.Update(); // 更新当前状态
        CheckforDashInput(); // 检查冲刺输入

        if (Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked) { // 检查F键和水晶技能是否解锁
            skill.crystal.CanUseSkill(); // 使用水晶技能
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { // 检查数字键1是否按下
            Inventory.instance.UseFlask(); // 使用背包中的药水
        }
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage); // 减慢移动速度
        jumpForce = jumpForce * (1 - _slowPercentage); // 减慢跳跃力
        dashSpeed = dashSpeed * (1 - _slowPercentage); // 减慢冲刺速度
        anim.speed = anim.speed * (1 - _slowPercentage); // 减慢动画速度

        Invoke("ReturnDefaultSpeed", _slowDuration); // 在指定时间后恢复默认速度
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed(); // 调用基类的ReturnDefaultSpeed方法
        moveSpeed = defaultMoveSpeed; // 恢复默认移动速度
        jumpForce = defaultJumpForce; // 恢复默认跳跃力
        dashSpeed = defaultDashSpeed; // 恢复默认冲刺速度
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword; // 分配新的剑对象
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword); // 切换到接剑状态
        Destroy(sword); // 销毁当前剑对象
    }

    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true; // 设置玩家为忙碌状态

        yield return new WaitForSeconds(seconds); // 等待指定秒数

        isBusy = false; // 设置玩家为非忙碌状态
    }

    public void AnimationTrigger() => stateMachine.currenState.AnimationFinishTrigger(); // 触发当前状态的动画完成事件

    private void CheckforDashInput()
    {
        if (IsWallDetected()) // 检查是否检测到墙壁
        {
            return; // 如果检测到墙壁则返回
        }
        if(skill.dash.dashUnlocked == false) // 检查冲刺技能是否解锁
        {
            return; // 如果未解锁则返回
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill()) // 检查左Shift键和冲刺技能是否可用
        {
            dashDir = Input.GetAxisRaw("Horizontal"); // 获取水平输入方向
            if (dashDir == 0) // 如果没有水平输入
            {
                dashDir = facingDir; // 使用面朝方向作为冲刺方向
            }
            stateMachine.ChangeState(dashState); // 切换到冲刺状态
        }
    }

    public override void Die()
    {
        base.Die(); // 调用基类的Die方法

        stateMachine.ChangeState(deadState); // 切换到死亡状态
    }

    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower =  Vector2.zero; // 设置击退力量为零
    }
}
