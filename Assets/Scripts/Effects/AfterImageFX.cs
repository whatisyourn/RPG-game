using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统集合泛型命名空间
using UnityEngine; // 引入Unity引擎命名空间

public class AfterImageFX : MonoBehaviour // AfterImageFX类，继承自MonoBehaviour
{
    private SpriteRenderer sr; // 精灵渲染器组件
    private float colorLooseRate; // 颜色衰减速率

    // SetupAfterImage方法用于设置残影效果
    public void SetupAfterImage(float _loosingSpeed, Sprite _spriteImage) {
        sr = GetComponent<SpriteRenderer>(); // 获取当前对象的SpriteRenderer组件
        sr.sprite = _spriteImage; // 设置精灵图像
        colorLooseRate = _loosingSpeed; // 设置颜色衰减速率
    }

    // Update方法每帧调用一次，用于更新对象状态
    private void Update()
    {
        float alpha = sr.color.a - colorLooseRate * Time.deltaTime; // 计算新的alpha值
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha); // 设置新的颜色值

        if(sr.color.a <= 0) // 如果alpha值小于等于0
        {
            Destroy(gameObject); // 销毁当前对象
        }
    }
}
