using UnityEngine; // 引入Unity引擎命名空间

// Checkpoint类用于管理游戏中的检查点
public class Checkpoint : MonoBehaviour
{
    private Animator anim; // 动画组件的引用
    public string id; // 检查点的唯一标识符
    public bool activationStatus; // 检查点的激活状态

    // Start方法在游戏开始时调用，用于初始化动画组件
    private void Start()
    {
        anim = GetComponent<Animator>(); // 获取动画组件
    }

    [ContextMenu("Generate checkpoint id")] // 在上下文菜单中添加生成检查点ID的选项

    // GenerateId方法用于生成检查点的唯一标识符
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString(); // 生成新的GUID并赋值给id
    }

    // OnTriggerEnter2D方法在其他碰撞体进入触发器时调用
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null) // 如果碰撞体是玩家
        {
            ActivateCheckpoint(); // 激活检查点
            SaveManager.instance.SaveGame(); // 保存游戏
        }
    }

    // ActivateCheckpoint方法用于激活检查点
    public void ActivateCheckpoint()
    {
        activationStatus = true; // 设置激活状态为true
        anim.SetBool("active", true); // 设置动画参数"active"为true
    }
}
