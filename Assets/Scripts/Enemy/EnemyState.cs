using UnityEngine; // 引入Unity引擎命名空间

// 定义EnemyState类，用于表示敌人的状态
public class EnemyState
{
    // Rigidbody2D组件，用于处理物理行为
    protected Rigidbody2D rb;

    // 敌人的基础实体，用于访问敌人的属性和方法
    protected Enemy enemyBase;
    // 敌人的状态机，用于管理敌人的状态转换
    protected EnemyStateMachine stateMachine;

    // 动画布尔参数名称，用于控制动画状态
    public string animBoolName;

    // 状态计时器，用于跟踪状态的持续时间
    protected float stateTimer;
    // 触发器标志，用于标记动画是否完成
    protected bool triggerCalled;

    // EnemyState的构造函数，初始化敌人的状态
    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachin, string _animBoolName)
    {
        // 初始化敌人的基础实体
        enemyBase = _enemyBase;
        // 初始化状态机
        stateMachine = _stateMachin;
        // 初始化动画布尔参数名称
        animBoolName = _animBoolName;
    }

    // Enter方法：在进入状态时调用
    public virtual void Enter()
    {
        // 将触发器标志设为false
        triggerCalled = false;
        // 设置动画参数为true以激活当前状态的动画
        enemyBase.anim.SetBool(animBoolName, true);
        // 获取敌人的Rigidbody2D组件
        rb = enemyBase.rb;
    }

    // Update方法：每帧调用一次，用于更新状态
    public virtual void Update()
    {
        // 减少状态计时器的时间
        stateTimer -= Time.deltaTime;
    }

    // Exit方法：在退出状态时调用
    public virtual void Exit()
    {
        // 设置动画参数为false以关闭当前状态的动画
        enemyBase.anim.SetBool(animBoolName, false);
        // 分配最后一个动画名称
        enemyBase.AssignLasAnimName(animBoolName);
    }

    // 动画完成时调用的方法
    public virtual void AnimationFinishTrigger()
    {
        // 将触发器标志设为true，表示动画完成
        triggerCalled = true;
    }
}