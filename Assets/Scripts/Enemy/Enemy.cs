using System.Collections; // 引入System.Collections命名空间，用于协程操作
using UnityEngine; // 引入UnityEngine命名空间，提供Unity相关的类和方法

// Enemy类：继承自Entity类，表示游戏中的敌人实体
public class Enemy : Entity
{
    // 定义一个LayerMask，用于检测玩家所在的层
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Stun info")] // 在Inspector中为眩晕信息分组
    // 眩晕持续时间
    public float stunDuration;
    // 眩晕时的方向
    public Vector2 stunDirction;
    // 是否可以被眩晕
    protected bool canBeStunned;
    // 眩晕时显示的图像
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")] // 在Inspector中为移动信息分组
    // 移动速度
    public float moveSpeed;
    // 空闲时间
    public float idleTime;
    // 战斗时间
    public float battleTime;
    // 默认移动速度
    private float defalultMoveSpeed;

    [Header("Attack info")] // 在Inspector中为攻击信息分组
    // 攻击距离
    public float attackDistance;
    // 攻击冷却时间
    public float attackCooldown;
    public float minAttackCooldown;
    public float maxAttackCooldown;
    // 上次攻击时间
    [HideInInspector] public float lastTimeAttacked;
    // 敌人状态机
    public EnemyStateMachine stateMachine { get; private set; }
    // 实体特效
    public EntityFX fx { get; private set; }
    // 上一个动画布尔参数名称
    public string lastAnimBoolName { get; private set; }

    // Awake方法：初始化敌人状态机和默认移动速度
    protected override void Awake()
    {
        base.Awake(); // 调用基类的Awake方法
        stateMachine = new EnemyStateMachine(); // 初始化状态机

        defalultMoveSpeed = moveSpeed; // 设置初始速度
    }

    // Start方法：获取EntityFX组件
    protected override void Start()
    {
        base.Start(); // 调用基类的Start方法
        fx = GetComponent<EntityFX>(); // 获取EntityFX组件
    }

    // Update方法：每帧调用一次，用于更新当前状态
    protected override void Update()
    {
        base.Update(); // 调用基类的Update方法
        stateMachine.currentState.Update(); // 更新当前状态
    }

    // AssignLasAnimName方法：分配最后一个动画布尔参数名称
    public virtual void AssignLasAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;

    // SlowEntityBy方法：通过百分比和持续时间减慢实体速度
    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage); // 减慢移动速度
        anim.speed = anim.speed * (1 - _slowPercentage); // 减慢动画速度

        Invoke("ReturnDefaultSpeed", _slowDuration); // 调用ReturnDefaultSpeed方法恢复速度
    }

    // ReturnDefaultSpeed方法：恢复默认速度
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed(); // 调用基类的ReturnDefaultSpeed方法
        moveSpeed = defalultMoveSpeed; // 恢复默认移动速度
    }

    // FreezeTime方法：冻结或解冻时间
    public virtual void FreezeTime(bool _timeFrozen)
    {
        // 如果时间被冻结，则停止移动和动画
        if (_timeFrozen)
        {
            moveSpeed = 0; // 停止移动
            anim.speed = 0; // 停止动画
        }
        else
        {
            moveSpeed = defalultMoveSpeed; // 恢复默认移动速度
            anim.speed = 1; // 恢复动画速度
        }
    }

    // FreezeTimeFor方法：冻结时间一段持续时间
    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimerCoroutine(_duration));

    // FreezeTimerCoroutine协程：冻结时间的协程
    protected virtual IEnumerator FreezeTimerCoroutine(float _seconds)
    {
        FreezeTime(true); // 冻结时间
        yield return new WaitForSeconds(_seconds); // 等待指定秒数
        FreezeTime(false); // 解冻时间
    }

    #region Counter Attack Window
    // OpenCounterrAttackWindow方法：打开反击窗口
    public virtual void OpenCounterrAttackWindow()
    {
        canBeStunned = true; // 设置为可被眩晕
        counterImage.SetActive(true); // 打开反击窗口
    }

    // CloseCounterrAttackWindow方法：关闭反击窗口
    public virtual void CloseCounterrAttackWindow()
    {
        canBeStunned = false; // 设置为不可被眩晕
        counterImage.SetActive(false); // 关闭反击窗口
    }
    #endregion

    // CanBeStunned方法：检查敌人是否可以被眩晕
    public virtual bool CanBeStunned()
    {
        // 如果可以被眩晕，则关闭反击窗口并返回true
        if (canBeStunned)
        {
            CloseCounterrAttackWindow(); // 关闭反击窗口

            return true; // 返回true
        }
        return false; // 返回false
    }

    // AnimationTrigger方法：触发当前状态的动画完成触发器
    public virtual void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    // IsPlayerDetected方法：检测玩家是否在攻击范围内
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(transform.position, Vector2.right * facingDir, attackDistance, whatIsPlayer);

    // OnDrawGizmos方法：在编辑器中绘制Gizmos以显示攻击范围
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos(); // 调用基类的OnDrawGizmos方法

        Gizmos.color = Color.yellow; // 设置Gizmos颜色为黄色
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y)); // 绘制攻击范围线
    }
}