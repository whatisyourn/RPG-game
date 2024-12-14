using System.Collections; // 引入系统集合命名空间
using Cinemachine; // 引入Cinemachine命名空间
using TMPro; // 引入TextMeshPro命名空间
using Unity.VisualScripting; // 引入Unity Visual Scripting命名空间
using UnityEngine; // 引入Unity引擎命名空间

/// EntityFX类：用于管理实体的特效类，包括闪光特效、状态特效等
public class EntityFX : MonoBehaviour
{
    /// SpriteRenderer组件，用于修改实体的渲染效果
    protected SpriteRenderer sr;
    protected Player player; // 玩家对象

    [Header("Pop Up Text")]
    [SerializeField] private GameObject popUpTextPrefab; // 弹出文本的预制体

    /// 闪光特效相关变量
    [Header("Flash FX")]
    [SerializeField] private float flashDuration; // 闪光持续时间
    [SerializeField] private Material hitMat; // 闪光时的材质
    private Material originalMat; // 原始材质

    [Header("Ailment colors")]
    [SerializeField] private Color[] igniteColor; // 点燃状态的颜色
    [SerializeField] private Color[] chillColor; // 冰冻状态的颜色
    [SerializeField] private Color[] shockColor; // 震击状态的颜色

    [Header("Ailment particles")]
    [SerializeField] private ParticleSystem igniteFx; // 点燃状态的粒子效果
    [SerializeField] private ParticleSystem chillFx; // 冰冻状态的粒子效果
    [SerializeField] private ParticleSystem shockFx; // 震击状态的粒子效果

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFx; // 普通命中特效
    [SerializeField] private GameObject criticalHitFx; // 暴击命中特效

    /// Start方法：在对象初始化时调用，用于初始化特效相关组件和变量
    protected virtual void Start()
    {
        // 获取SpriteRenderer组件
        sr = GetComponentInChildren<SpriteRenderer>();
        // 保存原始材质
        originalMat = sr.material;
        // 获取玩家对象
        player = PlayerManager.instance.player;
    }

    /// Update方法：每帧调用一次，用于更新对象状态
    protected virtual void Update()
    {
        // 此处没有具体实现
    }

    /// CreatePopUpText方法：创建弹出文本
    /// <param name="_text">要显示的文本内容</param>
    public void CreatePopUpText(string _text)
    {
        // 随机生成文本位置偏移量
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(3, 5);
        Vector3 positionOffset = new Vector3(randomX, randomY, 0);
        // 实例化弹出文本预制体
        GameObject newText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity);
        // 设置文本内容
        newText.GetComponent<TextMeshPro>().text = _text;
    }

    /// MakeTransparent方法：设置角色透明度
    /// <param name="_transparent">是否透明</param>
    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            // 设置为透明
            sr.color = Color.clear;
        }
        else
        {
            // 设置为不透明
            sr.color = Color.white;
        }
    }

    /// FlashFX协程：实现被击中时的闪光特效
    /// <returns>协程</returns>
    private IEnumerator FlashFX()
    {
        // 保存当前颜色
        Color currentColor = sr.color;

        // 将材质更改为闪光时的材质，同时颜色改为白色，防止在点燃、冰冻或震击时颜色混淆
        sr.material = hitMat;
        sr.color = Color.white;

        // 等待闪光持续时间
        yield return new WaitForSeconds(flashDuration);
        // 恢复材质为原始材质，颜色恢复为原始颜色
        sr.material = originalMat;
        sr.color = currentColor;
    }

    /// RedColorBlink方法：实现红色闪烁特效，用于显示实体的某种状态
    private void RedColorBlink()
    {
        // 如果当前颜色不是白色，则切换为白色
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        // 否则切换为红色
        else
        {
            sr.color = Color.red;
        }
    }

    /// CancelColorChange方法：取消颜色闪烁特效
    private void CancelColorChange()
    {
        // 取消所有Invoke调用，停止闪烁特效
        CancelInvoke();
        // 颜色恢复为白色
        sr.color = Color.white;

        // 停止所有状态粒子效果
        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
    }

    /// IgniteFXFor方法：播放点燃特效
    /// <param name="_seconds">特效持续时间</param>
    public void IgniteFXFor(float _seconds)
    {
        // 播放点燃粒子效果
        igniteFx.Play();
        // 循环调用IgniteColorFX方法
        InvokeRepeating("IgniteColorFX", 0, .3f);
        // 在指定时间后取消颜色变化
        Invoke("CancelColorChange", _seconds);
    }

    /// ChillFXFor方法：播放冰冻特效
    /// <param name="_seconds">特效持续时间</param>
    public void ChillFXFor(float _seconds)
    {
        // 播放冰冻粒子效果
        chillFx.Play();
        // 循环调用ChillColorFX方法
        InvokeRepeating("ChillColorFX", 0, .3f);
        // 在指定时间后取消颜色变化
        Invoke("CancelColorChange", _seconds);
    }

    /// ShockFXFor方法：播放震击特效
    /// <param name="_seconds">特效持续时间</param>
    public void ShockFXFor(float _seconds)
    {
        // 播放震击粒子效果
        shockFx.Play();
        // 循环调用ShockColorFX方法
        InvokeRepeating("ShockColorFX", 0, .3f);
        // 在指定时间后取消颜色变化
        Invoke("CancelColorChange", _seconds);
    }

    /// IgniteColorFX方法：实现点燃状态的颜色变化
    private void IgniteColorFX()
    {
        // 如果当前颜色不是点燃颜色的第一种，则切换为第一种
        if (sr.color != igniteColor[0])
        {
            sr.color = igniteColor[0];
        }
        // 否则切换为第二种
        else
        {
            sr.color = igniteColor[1];
        }
    }

    /// ChillColorFX方法：实现冰冻状态的颜色变化
    private void ChillColorFX()
    {
        // 如果当前颜色不是冰冻颜色的第一种，则切换为第一种
        if (sr.color != chillColor[0])
        {
            sr.color = chillColor[0];
        }
        // 否则切换为第二种
        else
        {
            sr.color = chillColor[1];
        }
    }

    /// ShockColorFX方法：实现震击状态的颜色变化
    private void ShockColorFX()
    {
        // 如果当前颜色不是震击颜色的第一种，则切换为第一种
        if (sr.color != shockColor[0])
        {
            sr.color = shockColor[0];
        }
        // 否则切换为第二种
        else
        {
            sr.color = shockColor[1];
        }
    }

    /// CreateHitFx方法：创建命中特效
    /// <param name="_target">命中特效的目标位置</param>
    /// <param name="_critical">是否为暴击</param>
    public void CreateHitFx(Transform _target, bool _critical)
    {
        // 随机生成命中特效的旋转角度和位置偏移量
        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        Vector3 hitFxRotaion = new Vector3(0, 0, zRotation);
        GameObject hitPrefab = hitFx; // 默认使用普通命中特效
        if (_critical)
        {
            // 如果是暴击，使用暴击命中特效
            hitPrefab = criticalHitFx;

            float yRotation = 0;
            zRotation = Random.Range(-45, 45);
            if (GetComponent<Entity>().facingDir == -1)
                yRotation = 180;
            hitFxRotaion = new Vector3(0, yRotation, zRotation);
        }

        // 实例化命中特效，并设置其旋转角度
        GameObject newHits = Instantiate(hitPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);
        newHits.transform.Rotate(hitFxRotaion);

        // 在0.5秒后销毁命中特效
        Destroy(newHits, 0.5f);
    }
}