using System.Collections; // 引入系统集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// UI类用于管理游戏中的用户界面
public class UI : MonoBehaviour
{
    [Header("End screen")] // 在Inspector中为结束屏幕部分添加标题
    [SerializeField] private UI_FadeScreen fadeScreen; // 序列化字段，UI淡出屏幕组件
    [SerializeField] private GameObject endText; // 序列化字段，结束文本对象
    [SerializeField] private GameObject restartButton; // 序列化字段，重启按钮对象
    [Space] // 在Inspector中添加空行

    [SerializeField] private GameObject characterUI; // 序列化字段，角色UI对象
    [SerializeField] private GameObject skillTreeUI; // 序列化字段，技能树UI对象
    [SerializeField] private GameObject craftUI; // 序列化字段，制作UI对象
    [SerializeField] private GameObject optionsUI; // 序列化字段，选项UI对象
    [SerializeField] private GameObject inGameUI; // 序列化字段，游戏中UI对象

    public UI_SkillToolTip skillToolTip; // 技能提示工具对象
    public UI_ItemToolTip itemToolTip; // 物品提示工具对象
    public UI_StatToolTip statToolTip; // 属性提示工具对象
    public UI_CraftWindow craftWindow; // 制作窗口对象

    // Awake方法在脚本实例化时调用，用于初始化UI状态
    private void Awake()
    {
        // 切换到技能树UI并激活淡出屏幕
        SwitchTo(skillTreeUI);
        fadeScreen.gameObject.SetActive(true);
    }

    // Start方法在游戏开始时调用，用于设置初始UI状态
    void Start()
    {
        SwitchTo(inGameUI); // 切换到游戏中UI
        itemToolTip.gameObject.SetActive(false); // 隐藏物品提示工具
        statToolTip.gameObject.SetActive(false); // 隐藏属性提示工具
    }

    // Update方法每帧调用，用于检测用户输入并切换UI
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) // 检测按键C
        {
            SwitchWithKeyTo(characterUI); // 切换到角色UI
        }

        if (Input.GetKeyDown(KeyCode.B)) // 检测按键B
        {
            SwitchWithKeyTo(craftUI); // 切换到制作UI
        }

        if (Input.GetKeyDown(KeyCode.K)) // 检测按键K
        {
            SwitchWithKeyTo(skillTreeUI); // 切换到技能树UI
        }

        if (Input.GetKeyDown(KeyCode.O)) // 检测按键O
        {
            SwitchWithKeyTo(optionsUI); // 切换到选项UI
        }
    }

    // SwitchTo方法用于切换到指定的UI菜单
    public void SwitchTo(GameObject _menu)
    {
        // 关闭所有子对象的UI菜单
        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null; // 检查是否为淡出屏幕
            if (!fadeScreen)
                transform.GetChild(i).gameObject.SetActive(false); // 关闭非淡出屏幕的UI
        }
        // 激活指定的UI菜单
        if (_menu != null)
        {
            _menu.SetActive(true);
        }

        // 根据当前UI菜单状态暂停或继续游戏
        if (GameManager.instance != null) { 
            if(_menu == inGameUI)
            {
                GameManager.instance.PauseGame(false); // 继续游戏
            }
            else
            {
                GameManager.instance.PauseGame(true); // 暂停游戏
            }
        }
    }

    // SwitchWithKeyTo方法用于通过按键切换UI菜单
    public void SwitchWithKeyTo(GameObject _menu)
    {
        // 如果当前菜单已激活，则关闭它
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false); // 关闭菜单
            CheckForInGameUI(); // 检查是否需要切换到游戏中UI
            return;
        }
        SwitchTo(_menu); // 切换到指定菜单
    }

    // CheckForInGameUI方法用于检查是否需要切换到游戏中UI
    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            // 如果有其他UI菜单激活，则不切换到游戏中UI
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
            {
                return;
            }
        }
        SwitchTo(inGameUI); // 切换到游戏中UI
    }

    // SwitchOnEndScreen方法用于切换到结束屏幕
    public void SwitchOnEndScreen()
    {
        SwitchTo(null); // 关闭所有UI
        fadeScreen.FadeOut(); // 执行淡出效果
        StartCoroutine(EndScreenCorutione()); // 启动结束屏幕协程
    }

    // EndScreenCorutione协程用于显示结束屏幕
    IEnumerator EndScreenCorutione()
    {
        yield return new WaitForSeconds(1); // 等待1秒
        endText.SetActive(true); // 显示结束文本
        yield return new WaitForSeconds(1.5f); // 等待1.5秒
        restartButton.SetActive(true); // 显示重启按钮
    }

    // SaveAndExit方法用于保存游戏并退出
    public void SaveAndExit()
    {
        SaveManager.instance.SaveGame(); // 保存游戏
        Application.Quit(); // 退出应用程序
    }

    // RestartGameButton方法用于重启游戏场景
    public void RestartGameButton() => GameManager.instance.RestartScene(); // 重启场景
}
