using System; // 引入系统命名空间
using System.IO; // 引入IO命名空间，用于文件操作
using UnityEngine; // 引入Unity引擎命名空间

// 定义FileDataHandler类，用于处理文件的保存、加载和删除操作
public class FileDataHandler
{
    private string dataDirPath = ""; // 数据目录路径
    private string dataFileName = ""; // 数据文件名

    private bool encryptData = false; // 是否加密数据的标志
    private string codeWord = "adam"; // 用于加密和解密的密钥

    // 构造函数，初始化数据目录路径、文件名和加密标志
    public FileDataHandler(string _dataDirPath, string _dataFileName, bool _encryptData)
    {
        this.dataDirPath = _dataDirPath; // 设置数据目录路径
        this.dataFileName = _dataFileName; // 设置数据文件名
        this.encryptData = _encryptData; // 设置加密标志
    }

    // Save方法，用于将GameData对象保存到文件中
    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName); // 组合完整的文件路径

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)); // 创建目录（如果不存在）

            string dataToStore = JsonUtility.ToJson(_data, true); // 将GameData对象转换为JSON字符串
            
            if (encryptData) // 如果需要加密
            {
                dataToStore = EncryptDecrypt(dataToStore); // 加密数据
            }
            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) // 创建文件流
            {
                using (StreamWriter writer = new StreamWriter(stream)) // 创建流写入器
                {
                    writer.Write(dataToStore); // 写入数据到文件
                }
            }
        }
        catch (Exception e) // 捕获异常
        {
            Debug.LogError("Error on save to " + fullPath + "\n" + e); // 输出错误信息
        }
    }

    // Load方法，用于从文件中加载GameData对象
    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName); // 组合完整的文件路径
        GameData loadData = null; // 初始化加载的数据对象

        if (File.Exists(fullPath)) // 检查文件是否存在
        {
            try
            {
                string dataToLoad = ""; // 初始化要加载的数据字符串

                using (FileStream stream = new FileStream(fullPath, FileMode.Open)) // 打开文件流
                {
                    using (StreamReader reader = new StreamReader(stream)) // 创建流读取器
                    {
                        dataToLoad = reader.ReadToEnd(); // 读取文件内容
                    }
                }
                if (encryptData) // 如果需要解密
                {
                    dataToLoad = EncryptDecrypt(dataToLoad); // 解密数据
                }
                loadData = JsonUtility.FromJson<GameData>(dataToLoad); // 将JSON字符串转换为GameData对象
            }
            catch (Exception e) // 捕获异常
            {
                Debug.LogError("Error on load from " + fullPath + "\n" + e); // 输出错误信息
            }
        }
        return loadData; // 返回加载的数据对象
    }

    // Delete方法，用于删除数据文件
    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName); // 组合完整的文件路径

        if (File.Exists(fullPath)) // 检查文件是否存在
            File.Delete(fullPath); // 删除文件
    }

    // EncryptDecrypt方法，用于加密和解密字符串
    private string EncryptDecrypt(string _data)
    {
        string modifiedData = ""; // 初始化修改后的数据字符串

        for (int i = 0; i < _data.Length; i++) // 遍历数据字符串的每个字符
        {
            // 使用异或运算加密或解密字符，并添加到修改后的数据字符串中
            modifiedData += (char)(_data[i] ^ codeWord[i % codeWord.Length]);
        }
        return modifiedData; // 返回修改后的数据字符串
    }
}
