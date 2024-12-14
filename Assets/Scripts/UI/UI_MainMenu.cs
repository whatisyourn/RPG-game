using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间
using UnityEngine.SceneManagement; // 引入场景管理命名空间，用于场景切换

// UI_MainMenu类用于管理主菜单的UI交互
public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string screenName = "MainScene"; // 序列化字段，存储要加载的场景名称
    [SerializeField] private GameObject continueButton; // 序列化字段，继续游戏按钮的引用
    [SerializeField] private UI_FadeScreen fadeScreen; // 序列化字段，淡出屏幕效果的引用

    // Start方法在游戏开始时调用，用于初始化主菜单
    private void Start()
    {
        // 检查是否有保存数据，如果没有则禁用继续按钮
        if(SaveManager.instance.HasSavedData() == false)
        {
            continueButton.SetActive(false); // 禁用继续按钮
        }
    }

    // ContinueGame方法用于继续游戏
    public void ContinueGame()
    {
        // 启动协程以淡出效果加载场景
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    // NewGame方法用于开始新游戏
    public void NewGame()
    {
        // 删除保存数据
        SaveManager.instance.DeleteSaveData();
        // 启动协程以淡出效果加载场景
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    // ExitGame方法用于退出游戏
    public void ExitGame()
    {
        Debug.Log("Exit"); // 输出退出日志
    }

    // LoadSceneWithFadeEffect协程用于在淡出效果后加载场景
    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut(); // 执行淡出效果
        yield return new WaitForSeconds(_delay); // 等待指定的延迟时间
        SceneManager.LoadScene(screenName); // 加载指定的场景
    }
}
