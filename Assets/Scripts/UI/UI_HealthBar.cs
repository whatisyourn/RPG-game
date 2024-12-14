using UnityEngine; // 引入Unity引擎命名空间
using UnityEngine.UI; // 引入Unity UI命名空间，用于处理UI元素

// 定义UI_HealthBar类，继承自MonoBehaviour，用于管理角色的血条UI
public class UI_HealthBar : MonoBehaviour
{
    private Entity entity; // 定义Entity类型的私有变量entity，用于获取角色实体
    private CharacterStats myStats; // 定义CharacterStats类型的私有变量myStats，用于获取角色的状态信息
    private RectTransform myTransform; // 定义RectTransform类型的私有变量myTransform，用于获取UI的变换组件
    private Slider slider; // 定义Slider类型的私有变量slider，用于显示血条

    // Start方法在对象创建时调用，初始化血条UI
    private void Start()
    {
        entity = GetComponentInParent<Entity>(); // 获取父对象中的Entity组件并赋值给entity变量
        myTransform = GetComponent<RectTransform>(); // 获取当前对象的RectTransform组件并赋值给myTransform变量
        slider = GetComponentInChildren<Slider>(); // 获取子对象中的Slider组件并赋值给slider变量
        myStats = GetComponentInParent<CharacterStats>(); // 获取父对象中的CharacterStats组件并赋值给myStats变量

        // 订阅entity的onFlipped事件，当事件触发时调用FlipUI方法
        entity.onFlipped += FlipUI;
        // 订阅myStats的onHealthChanged事件，当事件触发时调用UpdateHealthUI方法
        myStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI(); // 初始化时更新血条UI
    }

    // UpdateHealthUI方法用于更新血条的显示
    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue(); // 设置slider的最大值为角色的最大生命值
        slider.value = myStats.currentHealth; // 设置slider的当前值为角色的当前生命值
    }

    // FlipUI方法用于翻转UI
    private void FlipUI() => myTransform.Rotate(0, 180, 0); // 旋转UI的Y轴180度

    // OnDisable方法在对象被禁用时调用，取消事件订阅
    private void OnDisable()
    {
        entity.onFlipped -= FlipUI; // 取消订阅entity的onFlipped事件
        myStats.onHealthChanged -= UpdateHealthUI; // 取消订阅myStats的onHealthChanged事件
    }
}
