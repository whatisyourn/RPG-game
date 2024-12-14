using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

[System.Serializable] // 标记类为可序列化，以便于保存和加载
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>(); // 序列化的键列表
    [SerializeField] private List<TValue> values = new List<TValue>(); // 序列化的值列表

    // 在序列化之前调用，将字典中的键值对存储到列表中
    public void OnBeforeSerialize()
    {
        keys.Clear(); // 清空键列表
        values.Clear(); // 清空值列表

        // 遍历字典中的每个键值对
        foreach(KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key); // 将键添加到键列表中
            values.Add(pair.Value); // 将值添加到值列表中
        }
    }

    // 在反序列化之后调用，将列表中的键值对重新添加到字典中
    public void OnAfterDeserialize()
    {
        this.Clear(); // 清空字典
        if(keys.Count != values.Count) // 检查键和值的数量是否匹配
        {
            Debug.Log("keys != value"); // 输出错误信息
        }

        // 遍历键列表，将键值对重新添加到字典中
        for(int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]); // 添加键值对到字典
        }
    }
}
