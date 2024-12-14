using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

[System.Serializable] // 标记GameData类为可序列化，以便于保存和加载
public class GameData 
{
    public int currency; // 玩家当前的货币数量

    public SerializableDictionary<string, bool> skillTree; // 玩家技能树，键为技能名称，值为技能是否解锁
    public SerializableDictionary<string, int> inventory; // 玩家物品栏，键为物品名称，值为物品数量
    public List<string> equipmentId; // 玩家装备的物品ID列表

    public SerializableDictionary<string, bool> checkpoints; // 游戏检查点，键为检查点ID，值为是否已激活
    public string closestCheckpointId; // 最近的检查点ID

    public int lostCurrencyAmount; // 玩家丢失的货币数量
    public float lostCurrencyX; // 玩家丢失货币的位置X坐标
    public float lostCurrencyY; // 玩家丢失货币的位置Y坐标

    // GameData构造函数，用于初始化游戏数据
    public GameData()
    {
        this.currency = 0; // 初始化货币数量为0
        this.lostCurrencyAmount = 0; // 初始化丢失货币数量为0
        this.lostCurrencyX = 0; // 初始化丢失货币位置X坐标为0
        this.lostCurrencyY = 0; // 初始化丢失货币位置Y坐标为0
        skillTree = new SerializableDictionary<string, bool>(); // 初始化技能树为空
        inventory = new SerializableDictionary<string, int>(); // 初始化物品栏为空
        equipmentId = new List<string>(); // 初始化装备ID列表为空
        closestCheckpointId = string.Empty; // 初始化最近检查点ID为空字符串
        checkpoints = new SerializableDictionary<string, bool>(); // 初始化检查点为空
    }
}
