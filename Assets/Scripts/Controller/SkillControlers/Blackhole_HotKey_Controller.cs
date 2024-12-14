using System.Collections; 
using System.Collections.Generic; 
using TMPro; 
using UnityEngine; 

public class Blackhole_HotKey_Controller : MonoBehaviour // 定义Blackhole_HotKey_Controller类，继承自MonoBehaviour
{
    private SpriteRenderer sr; // 声明一个私有的SpriteRenderer变量
    private KeyCode myHotKey; // 声明一个私有的KeyCode变量，用于存储热键
    private TextMeshProUGUI myText; // 声明一个私有的TextMeshProUGUI变量，用于显示热键文本

    private Transform myEnemy; // 声明一个私有的Transform变量，用于存储敌人的位置
    private Blackhole_Skill_Controller blackHole; // 声明一个私有的Blackhole_Skill_Controller变量，用于控制黑洞技能

    public void SetupHotKey(KeyCode _myNewHotKey, Transform _myEnemy, Blackhole_Skill_Controller _myBlackHole) // 定义一个公共方法，用于设置热键
    {
        myText = GetComponentInChildren<TextMeshProUGUI>(); // 获取子对象中的TextMeshProUGUI组件
        sr = GetComponent<SpriteRenderer>(); // 获取当前对象的SpriteRenderer组件
        myEnemy = _myEnemy; // 将传入的敌人位置赋值给myEnemy
        blackHole = _myBlackHole; // 将传入的黑洞技能控制器赋值给blackHole
        myHotKey = _myNewHotKey; // 将传入的热键赋值给myHotKey
        myText.text = _myNewHotKey.ToString(); // 将热键转换为字符串并显示在myText中
    }

    private void Update() // 定义一个私有的Update方法，每帧调用
    {
        if (Input.GetKeyDown(myHotKey)) // 检查是否按下了指定的热键
        {
            blackHole.AddEnemyToList(myEnemy); // 将敌人添加到黑洞技能的敌人列表中

            myText.color = Color.clear; // 将文本颜色设置为透明
            sr.color = Color.clear; // 将精灵颜色设置为透明
        }
    }

}
