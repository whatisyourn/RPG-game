using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统集合泛型命名空间
using Cinemachine; // 引入Cinemachine命名空间
using UnityEngine; // 引入Unity引擎命名空间

// PlayerFX类，继承自EntityFX，用于管理玩家的特效
public class PlayerFX : EntityFX
{
    [Header("Screen shake FX")] // 屏幕震动特效的设置
    [SerializeField] private CinemachineImpulseSource screenShake; // Cinemachine的震动源组件
    [SerializeField] private float shakeMultiplier; // 震动强度的倍数
    public Vector3 shakeSwordImpact; // 剑击时的震动强度
    public Vector3 shakeHighDamage; // 高伤害时的震动强度

    [Header("After image fx")] // 残影特效的设置
    [SerializeField] private GameObject afterPrefab; // 残影的预制体
    [SerializeField] private float colorLooseRate; // 颜色衰减速率
    [SerializeField] private float afterImageCooldown; // 残影生成的冷却时间
    private float afterImageCooldownTimer; // 残影冷却计时器

    [Space] // 在Inspector中增加空行
    [SerializeField] private ParticleSystem dustFX; // 尘土特效的粒子系统

    // Start方法：在对象初始化时调用，用于初始化特效相关组件
    protected override void Start()
    {
        base.Start(); // 调用父类的Start方法
        screenShake = GetComponent<CinemachineImpulseSource>(); // 获取CinemachineImpulseSource组件
    }

    // Update方法：每帧调用一次，用于更新对象状态
    protected override void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime; // 减少残影冷却计时器
    }

    // ScreenShake方法：触发屏幕震动效果
    // <param name="_shakePower">震动的强度向量</param>
    public void ScreenShake(Vector3 _shakePower)
    {
        // 设置震动的默认速度，考虑玩家的朝向和震动倍数
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse(); // 生成震动冲击
    }

    // CreateAfterImage方法：创建残影效果
    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer < 0) // 如果冷却时间已过
        {
            afterImageCooldownTimer = afterImageCooldown; // 重置冷却计时器
            // 实例化新的残影对象
            GameObject newAfterImage = Instantiate(afterPrefab, transform.position, transform.rotation);
            // 设置残影效果
            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLooseRate, sr.sprite);
        }
    }

    // PlayDustFX方法：播放尘土特效
    public void PlayDustFX()
    {
        if (dustFX != null) // 如果尘土特效存在
        {
            dustFX.Play(); // 播放尘土特效
        }
    }
}
