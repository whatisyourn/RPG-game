// 定义EnemyStateMachine类，用于管理敌人的状态机
public class EnemyStateMachine
{
    // 当前状态属性，只读，只能通过内部方法设置
    public EnemyState currentState { get; private set; }

    // 初始化状态的方法，用于设置初始状态并进入该状态
    public void Initialization(EnemyState StartState)
    {
        // 将当前状态设置为传入的初始状态
        currentState = StartState;
        // 调用当前状态的Enter方法，进入该状态
        currentState.Enter();
    }

    // 改变敌人状态的方法，用于在不同状态之间转换
    public void ChangeState(EnemyState _newState)
    {
        // 调用当前状态的Exit方法，退出当前状态
        currentState.Exit();
        // 将当前状态更新为传入的新状态
        currentState = _newState;
        // 调用新状态的Enter方法，进入新状态
        currentState.Enter();
    }
}