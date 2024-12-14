using System.Collections.Generic; // 引入系统泛型集合命名空间
using System.Linq; // 引入Linq命名空间，用于集合操作
using UnityEngine; // 引入Unity引擎命名空间

// SaveManager类，负责管理游戏的保存和加载
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance; // 静态实例，用于实现单例模式

    [SerializeField] private string fileName; // 保存文件的名称
    [SerializeField] private string filePath = "idbfs/adam951753sfcegjx"; // 保存文件的路径
    [SerializeField] private bool encryptData; // 是否加密数据的标志
    private GameData gameData; // 存储游戏数据的对象
    private List<ISaveManager> saveManagers; // 保存管理器的列表
    private FileDataHandler dataHandler; // 文件数据处理器

    // DeleteSaveData方法，用于删除保存的数据文件
    [ContextMenu("Delete save file")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(filePath, fileName, encryptData); // 初始化文件数据处理器
        dataHandler.Delete(); // 调用删除方法
    }

    // Awake方法，在脚本实例化时调用，用于初始化单例
    private void Awake()
    {
        if (instance != null) // 如果实例已存在
            Destroy(instance.gameObject); // 销毁旧实例
        else
            instance = this; // 设置当前实例为单例
    }

    // Start方法，在游戏开始时调用，用于初始化数据处理器和加载游戏
    private void Start()
    {
        dataHandler = new FileDataHandler(filePath, fileName, encryptData); // 初始化文件数据处理器
        saveManagers = FindAllSaveManagers(); // 查找所有保存管理器
        LoadGame(); // 加载游戏数据
    }

    // NewGame方法，用于初始化新的游戏数据
    public void NewGame()
    {
        gameData = new GameData(); // 创建新的GameData对象
    }

    // LoadGame方法，用于加载游戏数据
    public void LoadGame()
    {
        gameData = dataHandler.Load(); // 从文件加载游戏数据
        if (gameData == null) // 如果没有保存的数据
        {
            Debug.Log("No saving"); // 输出日志信息
            NewGame(); // 初始化新游戏
        }

        foreach (ISaveManager saveManager in saveManagers) // 遍历所有保存管理器
        {
            saveManager.LoadData(gameData); // 加载数据到每个保存管理器
        }
    }

    // SaveGame方法，用于保存游戏数据
    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers) // 遍历所有保存管理器
        {
            saveManager.SaveData(ref gameData); // 保存数据到每个保存管理器
        }
        dataHandler.Save(gameData); // 将游戏数据保存到文件
    }

    // OnApplicationQuit方法，在应用程序退出时调用，用于保存游戏数据
    private void OnApplicationQuit()
    {
        SaveGame(); // 保存游戏数据
    }

    // FindAllSaveManagers方法，用于查找所有实现了ISaveManager接口的对象
    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>(); // 查找所有ISaveManager

        return new List<ISaveManager>(saveManagers); // 返回保存管理器列表
    }

    // HasSavedData方法，用于检查是否存在保存的数据
    public bool HasSavedData()
    {
        if(dataHandler.Load() != null) // 如果加载的数据不为空
        {
            return true; // 返回true，表示存在保存的数据
        }
        return false; // 返回false，表示不存在保存的数据
    }
}
