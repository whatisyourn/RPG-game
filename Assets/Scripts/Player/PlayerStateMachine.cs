using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// PlayerStateMachine类用于管理玩家的状态，并在不同状态之间进行转换
public class PlayerStateMachine
{
    // 当前状态的属性，只能通过内部方法进行设置，外部只能获取
    public PlayerState currenState { get; private set; }

    // Initialize方法用于初始化状态机，设置初始状态并进入该状态
    public void Initialize(PlayerState _startState)
    {
        currenState = _startState; // 将当前状态设置为传入的初始状态
        currenState.Enter(); // 调用当前状态的Enter方法，执行进入状态的逻辑
    }

    // ChangeState方法用于在不同状态之间进行转换
    public void ChangeState(PlayerState _newState)
    {
        currenState.Exit(); // 调用当前状态的Exit方法，执行退出当前状态的逻辑
        currenState = _newState; // 将当前状态设置为新状态
        currenState.Enter(); // 调用新状态的Enter方法，执行进入新状态的逻辑
    }
}