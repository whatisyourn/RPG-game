using UnityEngine;

// ShockStrike_Controller类，继承自MonoBehaviour
public class ShockStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats; // 目标角色的状态
    [SerializeField] private float speed; // 移动速度
    private int damage; // 伤害值

    private Animator anim; // 动画组件
    private bool trigger; // 是否触发攻击

    // Start方法在第一次帧更新前调用，用于初始化
    void Start()
    {
        anim = GetComponentInChildren<Animator>(); // 获取子对象中的Animator组件
    }

    // Setup方法用于设置攻击的伤害值和目标角色状态
    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage; // 设置伤害值
        targetStats = _targetStats; // 设置目标角色状态
    }

    // Update方法每帧调用一次，用于更新对象状态
    void Update()
    {
        if (!targetStats) // 如果没有目标角色状态
        {
            return; // 直接返回
        }

        if (trigger) // 如果已经触发攻击
        {
            return; // 直接返回
        }

        // 移动到目标位置
        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        // 设置对象的朝向
        transform.right = transform.position - targetStats.transform.position;
        
        // 如果与目标的距离小于0.1
        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            anim.transform.localPosition = new Vector3(0, .5f); // 设置动画位置
            anim.transform.localRotation = Quaternion.identity; // 重置动画旋转

            transform.localRotation = Quaternion.identity; // 重置对象旋转
            transform.localScale = new Vector3(3, 3); // 设置对象缩放

            Invoke("DamageAndSelfDestroy", .5f); // 延迟0.5秒后调用DamageAndSelfDestroy方法
            trigger = true; // 设置触发标志为true
            anim.SetTrigger("Hit"); // 触发“Hit”动画
        }
    }

    // DamageAndSelfDestroy方法用于对目标造成伤害并销毁自身
    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true); // 对目标应用震击效果
        targetStats.TakeDamage(damage); // 对目标造成伤害
        Destroy(gameObject, .4f); // 在0.4秒后销毁自身
    }

}
