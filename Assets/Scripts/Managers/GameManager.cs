using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间
using UnityEngine.SceneManagement; // 引入Unity场景管理命名空间

// 定义GameManager类，继承自MonoBehaviour并实现ISaveManager接口
public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance; // 定义一个静态实例，用于实现单例模式
    private Transform player; // 定义一个Transform变量，用于存储玩家的Transform
    [SerializeField] private Checkpoint[] checkpoints; // 定义一个私有数组，用于存储所有的检查点
    [SerializeField] private string closestCheckpointId; // 定义一个私有字符串，用于存储最近检查点的ID

    [Header("Lost currency")] // 在Unity编辑器中显示的标题
    [SerializeField] private GameObject lostCurrencyPrefab; // 定义一个私有GameObject变量，用于存储丢失货币的预制体
    public int lostCurrencyAmount; // 定义一个整数变量，用于存储丢失货币的数量
    [SerializeField] private float lostCurrencyX; // 定义一个私有浮点变量，用于存储丢失货币的X坐标
    [SerializeField] private float lostCurrencyY; // 定义一个私有浮点变量，用于存储丢失货币的Y坐标

    // Awake函数，用于初始化单例模式和获取玩家及检查点信息
    private void Awake()
    {
        if (instance != null) // 如果实例不为空
            Destroy(instance.gameObject); // 销毁已有的实例对象
        else
            instance = this; // 将当前实例赋值给静态实例

        checkpoints = FindObjectsOfType<Checkpoint>(); // 获取场景中所有的检查点
        player = PlayerManager.instance.player.transform; // 获取玩家的Transform
    }

    // Start函数，当前未实现任何功能
    private void Start()
    {

    }

    // RestartScene函数，用于重新加载当前场景
    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene(); // 获取当前活动场景
        SceneManager.LoadScene(scene.name); // 重新加载当前场景
    }

    // LoadData函数，启动协程以延迟加载数据
    public void LoadData(GameData _data) => StartCoroutine(LoadWithDelay(_data));

    // SaveData函数，用于保存游戏数据
    public void SaveData(ref GameData _data)
    {
        _data.checkpoints.Clear(); // 清空数据中的检查点信息
        if(FindClosestCheckpoint() != null) // 如果找到最近的检查点
            _data.closestCheckpointId = FindClosestCheckpoint().id; // 保存最近检查点的ID

        _data.lostCurrencyAmount = lostCurrencyAmount; // 保存丢失货币的数量
        _data.lostCurrencyX = player.position.x; // 保存玩家的X坐标
        _data.lostCurrencyY = player.position.y; // 保存玩家的Y坐标
        foreach (var checkpoint in checkpoints) // 遍历所有检查点
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus); // 保存检查点的ID和激活状态
        }
    }

    // LoadWithDelay函数，协程，用于延迟加载游戏数据
    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f); // 等待0.1秒
        LoadCheckPoint(_data); // 加载检查点数据
        LoadClosestCheckpoint(_data); // 加载最近检查点数据
        LoadLostCurrency(_data); // 加载丢失货币数据
    }

    // LoadClosestCheckpoint函数，用于加载最近的检查点
    private void LoadClosestCheckpoint(GameData _data)
    {
        if (_data.closestCheckpointId == null) // 如果最近检查点ID为空
        {
            return; // 直接返回
        }
        closestCheckpointId = _data.closestCheckpointId; // 获取最近检查点ID
        foreach (Checkpoint checkpoint in checkpoints) // 遍历所有检查点
        {
            if (closestCheckpointId == checkpoint.id) // 如果找到匹配的检查点
            {
                player.position = checkpoint.transform.position; // 将玩家位置设置为检查点位置
            }
        }
    }

    // LoadCheckPoint函数，用于加载检查点的激活状态
    private void LoadCheckPoint(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints) // 遍历数据中的检查点信息
        {
            foreach (Checkpoint checkpoint in checkpoints) // 遍历所有检查点
            {
                if (checkpoint.id == pair.Key && pair.Value) // 如果检查点ID匹配且激活状态为真
                {
                    checkpoint.ActivateCheckpoint(); // 激活检查点
                }
            }
        }
    }

    // LoadLostCurrency函数，用于加载丢失的货币
    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount; // 获取丢失货币的数量
        lostCurrencyX = _data.lostCurrencyX; // 获取丢失货币的X坐标
        lostCurrencyY = _data.lostCurrencyY; // 获取丢失货币的Y坐标
        if (lostCurrencyAmount > 0) // 如果丢失货币数量大于0
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity); // 实例化丢失货币对象
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount; // 设置丢失货币的数量
        }
        lostCurrencyAmount = 0; // 重置丢失货币数量
    }

    // FindClosestCheckpoint函数，用于查找最近的激活检查点
    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity; // 初始化最近距离为无穷大
        Checkpoint closestCheckpoint = null; // 初始化最近检查点为空
        foreach (var checkpoint in checkpoints) // 遍历所有检查点
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position); // 计算玩家到检查点的距离
            if (distanceToCheckpoint < closestDistance && checkpoint.activationStatus) // 如果距离更近且检查点已激活
            {
                closestDistance = distanceToCheckpoint; // 更新最近距离
                closestCheckpoint = checkpoint; // 更新最近检查点
            }
        }
        return closestCheckpoint; // 返回最近检查点
    }

    // PauseGame函数，用于暂停或恢复游戏
    public void PauseGame(bool _pause)
    {
        if (_pause) // 如果需要暂停
        {
            Time.timeScale = 0; // 将时间缩放设置为0，暂停游戏
        }
        else // 如果需要恢复
        {
            Time.timeScale = 1; // 将时间缩放设置为1，恢复游戏
        }
    }

}
