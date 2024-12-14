using UnityEngine; // 引入Unity引擎命名空间

// 定义PlayerCatchSwordState类，继承自PlayerState类，用于处理玩家接剑的状态
public class PlayerCatchSwordState : PlayerState
{
    private Transform sword; // 定义Transform变量，用于存储剑的位置信息

    // 构造函数，用于初始化PlayerCatchSwordState类的实例
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    // Enter方法，进入该状态时调用，初始化状态相关的逻辑
    public override void Enter()
    {
        base.Enter(); // 调用基类的Enter方法，执行进入状态的通用逻辑
        sword = player.sword.transform; // 获取剑的Transform组件

        player.fx.PlayDustFX(); // 播放尘土效果
        player.fx.ScreenShake(player.fx.shakeSwordImpact); // 触发屏幕震动效果

        // 根据玩家和剑的位置判断是否需要翻转玩家的朝向
        if (player.transform.position.x > sword.position.x && player.facingDir == 1)
        {
            player.Flip(); // 如果玩家在剑的右侧且面朝右，则翻转朝向
        }
        else if (player.transform.position.x < sword.position.x && player.facingDir == -1)
        {
            player.Flip(); // 如果玩家在剑的左侧且面朝左，则翻转朝向
        }

        // 设置玩家的速度，使其向剑的方向移动
        rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDir, rb.velocity.y);
    }

    // Exit方法，退出该状态时调用，处理状态退出时的逻辑
    public override void Exit()
    {
        base.Exit(); // 调用基类的Exit方法，执行退出状态的通用逻辑
        player.StartCoroutine("BusyFor", .1f); // 启动协程，使玩家忙碌0.1秒，防止立即切换状态
    }

    // Update方法，每帧调用一次，用于更新状态逻辑
    public override void Update()
    {
        base.Update(); // 调用基类的Update方法，执行每帧更新的通用逻辑

        // 检查动画触发器是否被调用，如果是则切换到玩家的空闲状态
        if ((triggerCalled))
        {
            stateMachine.ChangeState(player.idleState); // 切换到玩家的空闲状态
        }
    }
}