using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// 定义ISaveManager接口，用于管理游戏数据的保存和加载
public interface ISaveManager
{
    // LoadData方法，用于从GameData对象中加载数据
    void LoadData(GameData _data);

    // SaveData方法，用于将数据保存到GameData对象中
    void SaveData(ref GameData _data);
}
