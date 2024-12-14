// 引入Unity引擎的命名空间，提供游戏开发所需的类和函数
using UnityEngine;

// 定义PlayerAimSwordState类，继承自PlayerState类，用于处理玩家瞄准剑的状态
public class PlayerAimSwordState : PlayerState
{
    // 构造函数，用于初始化PlayerAimSwordState类的实例
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    // Enter方法，进入该状态时调用，初始化状态相关的逻辑
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法，执行进入状态的通用逻辑

        // 激活剑技能的瞄准点，使其在屏幕上可见
        player.skill.sword.DotsActivate(true);
    }

    // Exit方法，退出该状态时调用，处理状态退出时的逻辑
    public override void Exit()
    {
        base.Exit(); // 调用基类的Exit方法，执行退出状态的通用逻辑

        // 启动一个协程，使玩家忙碌0.2秒，用于在状态切换之间创建延迟
        player.StartCoroutine("BusyFor", .2f);
    }

    // Update方法，每帧调用一次，用于更新状态逻辑
    public override void Update()
    {
        base.Update(); // 调用基类的Update方法，执行每帧更新的通用逻辑

        // 将玩家的速度设置为零，防止移动
        player.SetZeroVelocity();

        // 检查鼠标右键是否松开
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            // 如果鼠标右键松开，则切换到玩家的空闲状态
            stateMachine.ChangeState(player.idleState);
        }

        // 获取鼠标在屏幕上的位置，并转换为世界坐标
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 根据玩家和鼠标的位置判断是否需要翻转玩家的朝向
        if (player.transform.position.x > mousePosition.x && player.facingDir == 1)
        {
            // 如果玩家在鼠标右侧且面朝右，则翻转朝向
            player.Flip();
        }
        else if (player.transform.position.x < mousePosition.x && player.facingDir == -1)
        {
            // 如果玩家在鼠标左侧且面朝左，则翻转朝向
            player.Flip();
        }
    }
}