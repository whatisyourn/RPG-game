using System; // 引入系统命名空间

// 定义一个可序列化的类，用于表示库存物品
[Serializable] // 使类可序列化
public class InventoryItem
{
    public ItemData data; // 存储物品数据
    public int stackSize; // 存储物品堆叠数量

    // 构造函数，初始化物品数据并增加堆叠数量
    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData; // 将传入的物品数据赋值给data
        Addstack(); // 调用Addstack方法，增加堆叠数量
    }

    // 增加堆叠数量的方法
    public void Addstack() => stackSize++; // 堆叠数量加1

    // 减少堆叠数量的方法
    public void Removestack() => stackSize--; // 堆叠数量减1
}
