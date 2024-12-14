using System.Collections; // 引入系统集合命名空间
using Unity.VisualScripting; // 引入Visual Scripting命名空间
using UnityEngine; // 引入Unity引擎命名空间

// Entity类用于管理游戏中的实体对象
public class Entity : MonoBehaviour
{
    /// Entity: 用于游戏中的实体对象
    /// 功能包括: 碰撞检测、动画和物理的初始化

    #region Components
    public Animator anim { get; private set; } // 动画组件
    public Rigidbody2D rb { get; private set; } // 刚体组件
    public SpriteRenderer sr { get; private set; } // 精灵渲染组件
    public CharacterStats stats { get; private set; } // 角色属性组件
    public CapsuleCollider2D cd { get; private set; } // 胶囊碰撞体组件
    #endregion

    // 击退相关信息
    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackPower; // 击退力量
    [SerializeField] protected float knockbackDuration; // 击退持续时间
    protected bool isKnocked; // 是否正在被击退

    // 碰撞检测相关信息
    [Header("Collision info")]
    public Transform attackCheck; // 攻击检测
    public float attackCheckRadius; // 攻击检测半径
    [SerializeField] protected Transform groundCheck; // 地面检测
    [SerializeField] protected float groundCheckDistance; // 地面检测距离
    [SerializeField] protected LayerMask whatIsGround; // 地面层
    [SerializeField] protected Transform wallCheck; // 墙壁检测
    [SerializeField] protected float wallCheckDistance; // 墙壁检测距离

    public int knockbackDir { get; private set; } // 击退方向
    public int facingDir { get; private set; } = 1; // 面向方向
    protected bool facingRight = true; // 是否面向右

    public System.Action onFlipped; // 翻转事件

    // Awake方法在脚本实例化时调用，用于初始化组件
    protected virtual void Awake()
    {

    }

    // Start方法在游戏开始时调用，用于获取组件引用
    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>(); // 获取子对象中的SpriteRenderer组件
        anim = GetComponentInChildren<Animator>(); // 获取子对象中的Animator组件
        rb = GetComponent<Rigidbody2D>(); // 获取Rigidbody2D组件
        stats = GetComponent<CharacterStats>(); // 获取CharacterStats组件
        cd = GetComponent<CapsuleCollider2D>(); // 获取CapsuleCollider2D组件
    }

    // Update方法每帧调用，用于更新实体状态
    protected virtual void Update()
    {

    }

    // SlowEntityBy方法用于减慢实体的速度
    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {

    }

    // ReturnDefaultSpeed方法用于恢复默认速度
    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1; // 恢复动画速度
    }

    // DamageImpact方法用于处理受到伤害时的击退效果
    public virtual void DamageImpact() => StartCoroutine("HitKnockback");

    // SetupKnockbackDir方法用于设置击退方向
    public virtual void SetupKnockbackDir(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)
        {
            knockbackDir = -1; // 设置击退方向为左
        }
        else
        {
            knockbackDir = 1; // 设置击退方向为右
        }
    }

    // SetupKnockbackPower方法用于设置击退力量
    public void SetupKnockbackPower(Vector2 knockbackpower) => knockbackPower = knockbackpower;

    // HitKnockback方法用于处理击退效果的协程
    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true; // 设置为正在被击退
        rb.velocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y); // 设置刚体速度为击退力量
        yield return new WaitForSeconds(knockbackDuration); // 等待击退持续时间
        isKnocked = false; // 设置为不再被击退
        SetupZeroKnockbackPower(); // 重置击退力量
    }

    // SetupZeroKnockbackPower方法用于重置击退力量
    protected virtual void SetupZeroKnockbackPower()
    {

    }

    #region Velocity
    // SetZeroVelocity方法用于将速度设置为0
    public void SetZeroVelocity()
    {
        if (isKnocked)
        {
            return; // 如果正在被击退，则不执行
        }
        rb.velocity = new Vector2(0, 0); // 设置刚体速度为0
    }

    // SetVelocity方法用于设置速度
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
        {
            return; // 如果正在被击退，则不执行
        }
        rb.velocity = new Vector2(_xVelocity, _yVelocity); // 设置刚体速度
        FlipControl(_xVelocity); // 控制翻转
    }
    #endregion

    #region Collision
    // IsGroundDetected方法用于检测是否接触地面
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    // IsWallDetected方法用于检测是否接触墙壁
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    // OnDrawGizmos方法用于在编辑器中绘制辅助图形
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance)); // 绘制地面检测线
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y)); // 绘制墙壁检测线
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius); // 绘制攻击检测范围
    }
    #endregion

    #region Flip
    // Flip方法用于翻转实体
    public virtual void Flip()
    {
        facingDir = facingDir * -1; // 翻转面向方向
        facingRight = !facingRight; // 翻转是否面向右
        transform.Rotate(0, 180, 0); // 旋转实体
        if (onFlipped != null)
        {
            onFlipped(); // 触发翻转事件
        }
    }

    // FlipControl方法用于控制翻转
    public virtual void FlipControl(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip(); // 如果向右移动且未面向右，则翻转
        }
        else if (_x < 0 && facingRight)
        {
            Flip(); // 如果向左移动且面向右，则翻转
        }
    }
    #endregion

    // Die方法用于处理实体死亡
    public virtual void Die()
    {

    }
}
