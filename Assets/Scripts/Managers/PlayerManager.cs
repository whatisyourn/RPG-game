using UnityEngine; // 引入Unity引擎命名空间

// 定义PlayerManager类，继承自MonoBehaviour并实现ISaveManager接口
public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance; // 定义一个静态实例，用于实现单例模式
    public Player player; // 定义一个Player类型的公共变量，用于存储玩家对象

    public int currency; // 定义一个整数变量currency，用于存储玩家的货币数量
    private void Awake() // Awake方法用于初始化单例模式
    {
        if (instance != null) // 如果实例已经存在
            Destroy(instance.gameObject); // 销毁已有的实例对象
        else
            instance = this; // 将当前实例赋值给静态实例
    }

    // HaveEnoughtMoney方法用于检查玩家是否有足够的货币购买物品
    public bool HaveEnoughtMoney(int _price)
    {
        if(_price > currency) // 如果物品价格大于玩家当前的货币数量
        {
            Debug.Log("Not enought"); // 输出“Not enought”到控制台
            return false; // 返回false，表示货币不足
        }
        currency -= _price; // 从玩家的货币中扣除物品价格
        return true; // 返回true，表示货币足够
    }

    // GetCurrency方法用于获取玩家当前的货币数量
    public int GetCurrency() => currency; // 返回当前的货币数量

    // LoadData方法用于从GameData中加载玩家的货币数据
    public void LoadData(GameData _data)
    {
        this.currency = _data.currency; // 将GameData中的货币数据赋值给当前玩家的货币
    }

    // SaveData方法用于将玩家的货币数据保存到GameData中
    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency; // 将当前玩家的货币数据赋值给GameData
    }
}
