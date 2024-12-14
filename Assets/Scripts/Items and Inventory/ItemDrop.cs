using System.Collections.Generic; // 引入系统集合泛型命名空间
using UnityEngine; // 引入Unity引擎命名空间

// 定义一个ItemDrop类，继承自MonoBehaviour
public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop; // 定义一个私有整型变量possibleItemDrop，用于存储可能掉落的物品数量
    [SerializeField] private ItemData[] posssibleDrop; // 定义一个私有数组posssibleDrop，用于存储可能掉落的物品数据
    private List<ItemData> dropList = new List<ItemData>(); // 定义一个私有列表dropList，用于存储实际掉落的物品数据

    [SerializeField] private GameObject dropPrefab; // 定义一个私有GameObject变量dropPrefab，用于存储掉落物品的预制体

    // 定义一个虚方法GenerateDrop，用于生成掉落物品
    public virtual void GenerateDrop()
    {
        // 遍历所有可能掉落的物品
        for (int i = 0; i < posssibleDrop.Length; i++)
        {
            // 如果随机数小于等于物品的掉落几率，则将该物品添加到掉落列表中
            if (Random.Range(0, 100) <= posssibleDrop[i].dropChance)
            {
                dropList.Add(posssibleDrop[i]); // 将物品添加到掉落列表
            }
        }

        // 根据可能掉落的物品数量和掉落列表的数量，生成实际掉落的物品
        for (int i = 0; i < possibleItemDrop && dropList.Count > 0; i++)
        {
            // 从掉落列表中随机选择一个物品
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];
            dropList.Remove(randomItem); // 从掉落列表中移除该物品
            DropItem(randomItem); // 调用DropItem方法，生成掉落物品
        }
    }

    // 定义一个受保护的方法DropItem，用于生成掉落物品
    protected void DropItem(ItemData _itemData)
    {
        // 实例化一个新的掉落物品，位置为当前对象的位置，旋转为默认值
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        // 定义一个随机的二维速度向量
        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

        // 设置掉落物品的属性，包括物品数据和随机速度
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
