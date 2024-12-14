// Enemy_Skeleton类继承自Enemy类，表示骷髅敌人
public class Enemy_Skeleton : Enemy
{
    #region State
    // 定义不同状态的属性，使用私有set方法
    public SkeletonIdleState idleState { get; private set; } // 骷髅的空闲状态
    public SkeletonMoveState moveState { get; private set; } // 骷髅的移动状态
    public SkeletonBattleState battleState { get; private set; } // 骷髅的战斗状态
    public SkeletonAttackState attackState { get; private set; } // 骷髅的攻击状态
    public SkeletonStunnedState stunnedState { get; private set; } // 骷髅的眩晕状态
    public SkeletonDeadState deadState { get; private set; } // 骷髅的死亡状态
    #endregion

    // Awake方法用于初始化状态
    protected override void Awake()
    {
        base.Awake(); // 调用父类的Awake方法
        // 初始化各个状态，并将当前对象和状态机传入
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this); // 初始化空闲状态
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this); // 初始化移动状态
        battleState = new SkeletonBattleState(this, stateMachine, "Move", this); // 初始化战斗状态
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this); // 初始化攻击状态
        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned", this); // 初始化眩晕状态
        deadState = new SkeletonDeadState(this, stateMachine, "Idle", this); // 初始化死亡状态
    }

    // Start方法用于设置初始状态
    protected override void Start()
    {
        base.Start(); // 调用父类的Start方法
        // 初始化状态机，将初始状态设置为空闲状态
        stateMachine.Initialization(idleState);
    }

    // Update方法每帧调用一次，用于更新状态
    protected override void Update()
    {
        base.Update(); // 调用父类的Update方法
        // 由于状态更新逻辑在状态机的Update方法中已经处理，因此此处无需额外操作
    }

    // CanBeStunned方法用于判断敌人是否可以被眩晕
    public override bool CanBeStunned()
    {
        if (base.CanBeStunned()) // 调用父类方法判断是否可以被眩晕
        {
            stateMachine.ChangeState(stunnedState); // 如果可以，改变状态为眩晕状态
            return true; // 返回true表示可以被眩晕
        }
        return false; // 返回false表示不能被眩晕
    }

    // Die方法用于处理敌人死亡逻辑
    public override void Die()
    {
        base.Die(); // 调用父类的Die方法
        stateMachine.ChangeState(deadState); // 改变状态为死亡状态
    }
}