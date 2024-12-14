using System.Collections.Generic; // 引入系统集合泛型命名空间
using UnityEditor; // 引入Unity编辑器命名空间
using UnityEngine; // 引入Unity引擎命名空间

// 定义一个Inventory类，继承自MonoBehaviour和ISaveManager接口
public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance; // 定义一个静态实例，用于实现单例模式

    public List<ItemData> startingItems; // 定义一个列表，用于存储初始物品

    public List<InventoryItem> inventory; // 定义一个列表，用于存储库存物品
    public Dictionary<ItemData, InventoryItem> inventoryDictionary; // 定义一个字典，通过ItemData来判断库存中是否有该物品

    public List<InventoryItem> stash; // 定义一个列表，用于存储储藏物品
    public Dictionary<ItemData, InventoryItem> stashDictionary; // 定义一个字典，通过ItemData来判断储藏中是否有该物品

    public List<InventoryItem> equipment; // 定义一个列表，用于存储装备物品
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary; // 定义一个字典，通过ItemData_Equipment来判断装备中是否有该物品

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent; // 定义一个Transform变量，用于存储库存槽的父对象
    [SerializeField] private Transform stashSlotParent; // 定义一个Transform变量，用于存储储藏槽的父对象
    [SerializeField] private Transform equipSlotParent; // 定义一个Transform变量，用于存储装备槽的父对象
    [SerializeField] private Transform statSlotParent; // 定义一个Transform变量，用于存储状态槽的父对象

    private UI_ItemSlot[] inventoryItemSlot; // 定义一个UI_ItemSlot数组，用于存储库存槽
    private UI_ItemSlot[] stashItemSlot; // 定义一个UI_ItemSlot数组，用于存储储藏槽
    private UI_EquipmentSlot[] equipItemSlot; // 定义一个UI_EquipmentSlot数组，用于存储装备槽
    private UI_StatSlot[] statSlot; // 定义一个UI_StatSlot数组，用于存储状态槽

    [Header("Items cooldown")]
    private float lastTimeUsedFlask; // 定义一个浮点变量，用于存储上次使用药瓶的时间
    private float lastTimeUseArmor; // 定义一个浮点变量，用于存储上次使用护甲的时间
    public float flaskCooldown { get; private set; } // 定义一个只读属性，用于存储药瓶的冷却时间
    private float armorCooldown; // 定义一个浮点变量，用于存储护甲的冷却时间

    [Header("Data base")]
    public List<ItemData> itemDataBase; // 定义一个列表，用于存储物品数据库
    public List<InventoryItem> loadedItems; // 定义一个列表，用于存储加载的物品
    public List<ItemData_Equipment> loadedEquipment; // 定义一个列表，用于存储加载的装备

    // Awake函数，用于初始化单例模式
    public void Awake()
    {
        if (instance == null) // 如果实例为空
        {
            instance = this; // 将当前实例赋值给静态实例
        }
        else
        {
            Destroy(gameObject); // 如果实例不为空，销毁当前对象
        }
    }

    // Start函数，用于初始化库存、储藏和装备
    private void Start()
    {
        inventory = new List<InventoryItem>(); // 初始化库存列表
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>(); // 初始化库存字典

        stash = new List<InventoryItem>(); // 初始化储藏列表
        stashDictionary = new Dictionary<ItemData, InventoryItem>(); // 初始化储藏字典

        equipment = new List<InventoryItem>(); // 初始化装备列表
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>(); // 初始化装备字典

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>(); // 获取库存槽的子对象组件
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>(); // 获取储藏槽的子对象组件
        equipItemSlot = equipSlotParent.GetComponentsInChildren<UI_EquipmentSlot>(); // 获取装备槽的子对象组件
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>(); // 获取状态槽的子对象组件
        Invoke("AddStartingItems", .1f); // 延迟0.1秒调用AddStartingItems函数
    }

    // AddStartingItems函数，用于添加初始物品
    private void AddStartingItems()
    {
        foreach (ItemData_Equipment item in loadedEquipment) // 遍历加载的装备
        {
            EquipItem(item); // 装备物品
        }

        if (loadedItems.Count > 0) // 如果加载的物品数量大于0
        {
            foreach (InventoryItem item in loadedItems) // 遍历加载的物品
            {
                for (int i = 0; i < item.stackSize; i++) // 根据堆叠大小添加物品
                {
                    AddItem(item.data); // 添加物品
                }
            }
            return; // 返回
        }

        for (int i = 0; i < startingItems.Count; i++) // 遍历初始物品
        {
            if (startingItems[i] != null) // 如果初始物品不为空
            {
                AddItem(startingItems[i]); // 添加物品
            }
        }
    }

    // UpdateSlotUI函数，用于更新槽的UI
    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventoryItemSlot.Length; i++) // 遍历库存槽
        {
            inventoryItemSlot[i].CleanUpSlot(); // 清理槽
        }
        for (int i = 0; i < stashItemSlot.Length; i++) // 遍历储藏槽
        {
            stashItemSlot[i].CleanUpSlot(); // 清理槽
        }
        for (int i = 0; i < equipItemSlot.Length; i++) // 遍历装备槽
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary) // 遍历装备字典
            {
                if (item.Key.equipmentType == equipItemSlot[i].slotType) // 如果装备类型匹配
                {
                    equipItemSlot[i].UpdateSlot(item.Value); // 更新槽
                }
            }
        }

        for (int i = 0; i < inventory.Count; i++) // 遍历库存
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]); // 更新槽
        }
        for (int i = 0; i < stash.Count; i++) // 遍历储藏
        {
            stashItemSlot[i].UpdateSlot(stash[i]); // 更新槽
        }

        UpdateStatUI(); // 更新状态UI
    }

    // UpdateStatUI函数，用于更新状态UI
    public void UpdateStatUI()
    {
        for (int i = 0; i < statSlot.Length; i++) // 遍历状态槽
        {
            statSlot[i].UpdateStatValueUI(); // 更新状态值UI
        }
    }

    // AddItem函数，用于添加物品
    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemTtype.Equipment && CanAddItem()) // 如果物品类型是装备且可以添加物品
        {
            AddToInventory(_item); // 添加到库存
        }
        else if (_item.itemType == ItemTtype.Material) // 如果物品类型是材料
        {
            AddToStash(_item); // 添加到储藏
        }
        UpdateSlotUI(); // 更新槽UI
    }

    // EquipItem函数，用于装备物品
    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment; // 将物品转换为装备类型
        InventoryItem newItem = new InventoryItem(newEquipment); // 创建新的库存物品
        ItemData_Equipment oldEquipment = null; // 定义旧装备变量

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary) // 遍历装备字典
        {
            if (item.Key.equipmentType == newEquipment.equipmentType) // 如果装备类型匹配
            {
                oldEquipment = item.Key; // 赋值旧装备
            }
        }

        if (oldEquipment != null) // 如果旧装备不为空
        {
            UnequipItem(oldEquipment); // 卸下旧装备
            AddItem(oldEquipment); // 将旧装备添加到储藏
        }

        equipmentDictionary.Add(newEquipment, newItem); // 将新装备添加到装备字典
        equipment.Add(newItem); // 将新装备添加到装备列表
        newEquipment.AddModifiers(); // 添加装备修饰符

        RemoveItem(_item); // 从库存中移除新装备

        UpdateSlotUI(); // 更新槽UI
    }

    // UnequipItem函数，用于卸下装备
    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value)) // 尝试从装备字典中获取装备
        {
            equipment.Remove(value); // 从装备列表中移除装备
            equipmentDictionary.Remove(itemToRemove); // 从装备字典中移除装备
            itemToRemove.RemoveModifiers(); // 移除装备修饰符
        }
    }

    // AddToStash函数，用于添加物品到储藏
    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value)) // 尝试从储藏字典中获取物品
        {
            value.Addstack(); // 增加堆叠数量
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item); // 创建新的库存物品
            stash.Add(newItem); // 将新物品添加到储藏列表
            stashDictionary.Add(_item, newItem); // 将新物品添加到储藏字典
        }
    }

    // AddToInventory函数，用于添加物品到库存
    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value)) // 尝试从库存字典中获取物品
        {
            value.Addstack(); // 增加堆叠数量
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item); // 创建新的库存物品
            inventory.Add(newItem); // 将新物品添加到库存列表
            inventoryDictionary.Add(_item, newItem); // 将新物品添加到库存字典
        }
    }

    // RemoveItem函数，用于移除物品
    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value)) // 尝试从库存字典中获取物品
        {
            if (value.stackSize <= 1) // 如果堆叠数量小于等于1
            {
                inventory.Remove(value); // 从库存列表中移除物品
                inventoryDictionary.Remove(_item); // 从库存字典中移除物品
            }
            else
            {
                value.Removestack(); // 减少堆叠数量
            }
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue)) // 尝试从储藏字典中获取物品
        {
            if (stashValue.stackSize <= 1) // 如果堆叠数量小于等于1
            {
                stash.Remove(stashValue); // 从储藏列表中移除物品
                stashDictionary.Remove(_item); // 从储藏字典中移除物品
            }
            else
            {
                stashValue.Removestack(); // 减少堆叠数量
            }
        }

        UpdateSlotUI(); // 更新槽UI
    }

    // CanAddItem函数，用于判断是否可以添加物品
    public bool CanAddItem()
    {
        if (inventory.Count >= inventoryItemSlot.Length) // 如果库存数量大于等于库存槽数量
        {
            return false; // 返回false
        }
        return true; // 返回true
    }

    // CanCraft函数，用于判断是否可以制作物品
    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>(); // 定义一个列表，用于存储需要移除的材料

        for (int i = 0; i < _requiredMaterials.Count; i++) // 遍历所需材料
        {
            if (stashDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue)) // 尝试从储藏字典中获取材料
            {
                if (_requiredMaterials[i].stackSize <= stashValue.stackSize) // 如果所需材料数量小于等于储藏材料数量
                {
                    materialsToRemove.Add(stashValue); // 将材料添加到需要移除的列表中
                }
                else
                {
                    Debug.Log("not enought material 1"); // 输出调试信息
                    return false; // 返回false
                }
            }
            else
            {
                Debug.Log("not enought material 2"); // 输出调试信息
                return false; // 返回false
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++) // 遍历需要移除的材料
        {
            RemoveItem(materialsToRemove[i].data); // 移除材料
        }
        AddItem(_itemToCraft); // 添加制作的物品
        Debug.Log("Here is your item " + _itemToCraft.name); // 输出调试信息
        return true; // 返回true
    }

    // GetEquipmentList函数，用于获取装备列表
    public List<InventoryItem> GetEquipmentList() => equipment;

    // GetStashList函数，用于获取储藏列表
    public List<InventoryItem> GetStashList() => stash;

    // GetEquipment函数，用于获取指定类型的装备
    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipdIten = null; // 定义一个变量，用于存储装备

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary) // 遍历装备字典
        {
            if (item.Key.equipmentType == _type) // 如果装备类型匹配
            {
                equipdIten = item.Key; // 赋值装备
            }
        }

        return equipdIten; // 返回装备
    }

    // UseFlask函数，用于使用药瓶
    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask); // 获取当前药瓶
        if (currentFlask == null) // 如果当前药瓶为空
        {
            return; // 返回
        }
        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown; // 判断是否可以使用药瓶

        if (canUseFlask) // 如果可以使用药瓶
        {
            flaskCooldown = currentFlask.itemCooldown; // 设置药瓶冷却时间
            currentFlask.Effect(null); // 执行药瓶效果
            lastTimeUsedFlask = Time.time; // 更新上次使用药瓶的时间
        }
        else
        {
            Debug.Log("Flask on cooldown"); // 输出调试信息
        }
    }

    // UseArmor函数，用于使用护甲
    public bool UseArmor()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmentType.Armor); // 获取当前护甲
        if (currentArmor == null) // 如果当前护甲为空
        {
            return false; // 返回false
        }
        bool canUseArmor = Time.time > lastTimeUseArmor + armorCooldown; // 判断是否可以使用护甲

        if (canUseArmor) // 如果可以使用护甲
        {
            armorCooldown = currentArmor.itemCooldown; // 设置护甲冷却时间
            lastTimeUseArmor = Time.time; // 更新上次使用护甲的时间
            return true; // 返回true
        }
        else
        {
            Debug.Log("Armor on cooldown"); // 输出调试信息
            return false; // 返回false
        }
    }

    // LoadData函数，用于加载数据
    public void LoadData(GameData _data)
    {
        foreach (var pair in _data.inventory) // 遍历数据中的库存
        {
            foreach (var item in itemDataBase) // 遍历物品数据库
            {
                if (item != null && item.itemId == pair.Key) // 如果物品不为空且ID匹配
                {
                    InventoryItem itemToLoad = new InventoryItem(item); // 创建新的库存物品
                    itemToLoad.stackSize = pair.Value; // 设置堆叠数量

                    loadedItems.Add(itemToLoad); // 将物品添加到加载的物品列表中
                }
            }
        }

        foreach (string loadedItemId in _data.equipmentId) // 遍历数据中的装备ID
        {
            foreach (var item in itemDataBase) // 遍历物品数据库
            {
                if (item != null && item.itemId == loadedItemId) // 如果物品不为空且ID匹配
                {
                    loadedEquipment.Add(item as ItemData_Equipment); // 将物品添加到加载的装备列表中
                }
            }
        }
    }

    // SaveData函数，用于保存数据
    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear(); // 清空数据中的库存
        _data.equipmentId.Clear(); // 清空数据中的装备ID

        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary) // 遍历库存字典
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize); // 将物品ID和堆叠数量添加到数据中
        }
        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary) // 遍历储藏字典
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize); // 将物品ID和堆叠数量添加到数据中
        }

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary) // 遍历装备字典
        {
            _data.equipmentId.Add(pair.Key.itemId); // 将装备ID添加到数据中
        }
    }

#if UNITY_EDITOR

    // FillUpItemDataBase函数，用于填充物品数据库
    [ContextMenu("Fill up item data base")]
    private void FillUpItemDataBase() => itemDataBase = new List<ItemData>(GetItemDataBase());

    // GetItemDataBase函数，用于获取物品数据库
    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>(); // 定义一个列表，用于存储物品数据库
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" }); // 获取物品资源的GUID

        foreach (string SOName in assetNames) // 遍历资源GUID
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName); // 获取资源路径
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath); // 加载资源
            itemDataBase.Add(itemData); // 将资源添加到物品数据库中
        }
        return itemDataBase; // 返回物品数据库
    }
#endif
}
