using TMPro; // 引入TextMeshPro命名空间，用于处理文本渲染
using UnityEngine; // 引入Unity引擎命名空间

// PopUpTextFX类：用于管理弹出文本效果
public class PopUpTextFX : MonoBehaviour
{
    private TextMeshPro myTest; // TextMeshPro组件，用于显示文本

    [SerializeField] private float speed; // 文本移动速度
    [SerializeField] private float disappearanceSpeed; // 文本消失时的移动速度
    [SerializeField] private float colorDisappearanceSpeed; // 文本颜色消失速度

    [SerializeField] private float lifeTime; // 文本的生命周期
    private float textTimer; // 文本计时器，用于跟踪文本存在时间

    // Start方法：在对象初始化时调用，用于初始化文本组件和计时器
    void Start()
    {
        myTest = GetComponent<TextMeshPro>(); // 获取TextMeshPro组件
        textTimer = lifeTime; // 初始化文本计时器为生命周期
    }

    // Update方法：每帧调用一次，用于更新文本位置和透明度
    void Update()
    {
        // 移动文本位置，向上移动，速度为speed
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), speed * Time.deltaTime);
        textTimer -= Time.deltaTime; // 减少计时器时间
        if (textTimer < 0) // 如果计时器小于0，开始处理文本消失
        {
            // 计算新的alpha值，减少文本透明度
            float alpha = myTest.color.a - colorDisappearanceSpeed * Time.deltaTime;

            // 设置新的文本颜色
            myTest.color = new Color(myTest.color.r, myTest.color.g, myTest.color.b, alpha);

            // 如果alpha值小于50，改变文本移动速度为消失速度
            if (myTest.color.a < 50)
            {
                speed = disappearanceSpeed;
            }

            // 如果alpha值小于等于0，销毁文本对象
            if (myTest.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
