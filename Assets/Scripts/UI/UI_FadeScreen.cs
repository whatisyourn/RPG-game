using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// 定义UI_FadeScreen类，继承自MonoBehaviour
public class UI_FadeScreen : MonoBehaviour
{
    public Animator anim; // 定义Animator类型的公共变量anim，用于控制动画
    void Start() // Start方法在对象创建时调用
    {
        anim = GetComponent<Animator>(); // 获取当前对象的Animator组件并赋值给anim变量
    }

    // FadeOut方法用于触发淡出动画
    public void FadeOut() => anim.SetTrigger("fadeOut"); // 设置Animator的触发器为"fadeOut"
    
    // FadeIn方法用于触发淡入动画
    public void FadeIn() => anim.SetTrigger("fadeIn"); // 设置Animator的触发器为"fadeIn"
}
