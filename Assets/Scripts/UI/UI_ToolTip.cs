using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using TMPro; // 引入TextMeshPro命名空间，用于处理UI文本
using UnityEngine; // 引入Unity引擎命名空间

// UI_ToolTip类用于管理和显示工具提示的UI组件
public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] private float xLimit = 960; // 序列化字段，x轴的限制值
    [SerializeField] private float yLimit = 540; // 序列化字段，y轴的限制值

    [SerializeField] private float xOffset = 150; // 序列化字段，x轴的偏移量
    [SerializeField] private float yOffset = 100; // 序列化字段，y轴的偏移量

    // AdjustPosition方法用于根据鼠标位置调整工具提示的位置
    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition; // 获取当前鼠标位置

        float newXoffeset = 0, newYoffeset = 0; // 初始化新的x和y偏移量
        if (mousePosition.x > xLimit) // 如果鼠标x位置超过限制
        {
            newXoffeset = -xOffset; // 设置x偏移量为负值
        }
        else
        {
            newXoffeset = xOffset; // 否则设置x偏移量为正值
        }
        if (mousePosition.y > yLimit) // 如果鼠标y位置超过限制
        {
            newYoffeset = -yOffset; // 设置y偏移量为负值
        }
        else
        {
            newYoffeset = yOffset; // 否则设置y偏移量为正值
        }
        // 设置工具提示的位置为鼠标位置加上新的偏移量
        transform.position = new Vector2(mousePosition.x + newXoffeset, mousePosition.y + newYoffeset);
    }

    // AdjustFontSize方法用于根据文本长度调整字体大小
    public void AdjustFontSize(TextMeshProUGUI _text)
    {
        if (_text.text.Length > 12) { // 如果文本长度大于12
            _text.fontSize = _text.fontSize * .8f; // 将字体大小缩小为原来的80%
        }
    }
}
