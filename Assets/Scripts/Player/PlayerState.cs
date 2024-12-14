using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// 定义PlayerState类，作为所有玩家状态的基类
public class PlayerState
{
    // 保护的PlayerStateMachine变量，用于管理状态之间的转换
    protected PlayerStateMachine stateMachine;
    // 保护的Player变量，用于访问玩家的属性和方法
    protected Player player;

    // 保护的Rigidbody2D变量，用于处理玩家的物理行为
    protected Rigidbody2D rb;

    // 保护的浮点数变量，用于存储玩家的水平和垂直输入
    protected float xInput;
    protected float yInput;
    // 私有的字符串变量，用于存储动画布尔参数的名称
    private string animBoolName;

    // 保护的浮点数变量，用于状态计时
    protected float stateTimer;
    // 保护的布尔变量，标记动画触发器是否已被调用
    protected bool triggerCalled;

    // PlayerState的构造函数，用于初始化状态
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player; // 初始化玩家对象
        this.stateMachine = _stateMachine; // 初始化状态机
        this.animBoolName = _animBoolName; // 初始化动画布尔参数名称
    }

    // Enter方法，进入状态时调用，用于初始化状态
    public virtual void Enter()
    {
        rb = player.rb; // 获取玩家的Rigidbody2D组件
        player.anim.SetBool(animBoolName, true); // 设置动画布尔参数为true
        triggerCalled = false; // 重置动画触发器标记
    }

    // Update方法，每帧调用一次，用于更新状态逻辑
    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal"); // 获取水平输入
        yInput = Input.GetAxisRaw("Vertical"); // 获取垂直输入
        stateTimer -= Time.deltaTime; // 减少状态计时器
        player.anim.SetFloat("yVelocity", rb.velocity.y); // 设置动画的yVelocity参数
    }

    // Exit方法，退出状态时调用，用于清理状态
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false); // 设置动画布尔参数为false
    }

    // AnimationFinishTrigger方法，动画完成时调用
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true; // 设置动画触发器标记为true
    }
}