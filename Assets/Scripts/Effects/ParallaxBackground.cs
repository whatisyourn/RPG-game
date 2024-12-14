using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

// ParallaxBackground类：用于创建一个简单的视差滚动效果的类
public class ParallaxBackground : MonoBehaviour
{
    // 摄像机对象，用于获取摄像机的位置
    private GameObject cam;
    // 视差效果强度，控制背景移动的速度
    [SerializeField] private float parallaxEffect;

    // 存储背景的x轴位置和宽度
    private float xPosition;
    private float length;

    // Start方法：在游戏对象实例化时调用一次，用于初始化
    void Start()
    {
        // 通过标签"Main Camera"找到摄像机对象
        cam = GameObject.Find("Main Camera");
        // 存储当前背景的x轴位置
        xPosition = transform.position.x;
        // 获取当前背景图像的宽度
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update方法：每帧调用一次，用于更新背景的位置
    void Update()
    {
        // 根据摄像机的x位置和视差效果强度计算背景需要移动的距离
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        // 更新背景的位置，使其产生视差滚动效果
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        // 实现无缝滚动效果
        // 计算背景相对于摄像机需要移动的距离
        float distanceMove = cam.transform.position.x * (1 - parallaxEffect);
        // 如果背景已经完全离开摄像机的右侧，则向前移动一个图像的长度，实现循环
        if (distanceMove > length + xPosition)
        {
            xPosition = xPosition + length;
        }
        // 如果背景已经完全离开摄像机的左侧，则向后移动一个图像的长度，实现循环
        else if (distanceMove < xPosition - length)
        {
            xPosition = xPosition - length;
        }
    }
}