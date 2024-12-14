using System.Text; // 引入System.Text命名空间，用于处理字符串构建

using UnityEngine; // 引入UnityEngine命名空间
#if UNITY_EDITOR
using UnityEditor; // 引入UnityEditor命名空间，仅在Unity编辑器中可用
#endif

// 定义一个枚举类型ItemTtype，用于表示物品类型
public enum ItemTtype
{
    Material, // 材料类型
    Equipment // 装备类型
}

// 使用CreateAssetMenu属性创建一个可在Unity编辑器中创建的物品数据脚本对象
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject // 定义一个ItemData类，继承自ScriptableObject
{
    public ItemTtype itemType; // 定义一个变量itemType，用于存储物品类型
    public string itemName; // 定义一个变量itemName，用于存储物品名称
    public Sprite icon; // 定义一个变量icon，用于存储物品图标
    public string itemId; // 定义一个变量itemId，用于存储物品ID

    [Range(0, 100)]
    public float dropChance; // 定义一个变量dropChance，用于存储物品掉落几率，范围在0到100之间

    protected StringBuilder sb = new StringBuilder(); // 定义一个StringBuilder对象sb，用于构建字符串

    // OnValidate方法在编辑器中更改脚本对象时调用，用于生成物品ID
    private void OnValidate()
    {
        // 为当前物品生成一个唯一的ID
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this); // 获取当前物品的资源路径
        itemId = AssetDatabase.AssetPathToGUID(path); // 将资源路径转换为GUID并赋值给itemId
#endif
    }

    // GetDescription方法用于获取物品的描述信息，返回一个空字符串
    public virtual string GetDescription()
    {
        return ""; // 返回空字符串
    }
}
