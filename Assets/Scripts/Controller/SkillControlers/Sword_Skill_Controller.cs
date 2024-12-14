using System.Collections.Generic;
using UnityEngine;

// 剑技能控制器类，继承自MonoBehaviour
public class Sword_Skill_Controller : MonoBehaviour
{
    // 动画组件
    private Animator anim; // 动画组件
    private Rigidbody2D rb; // 刚体组件
    private CircleCollider2D cd; // 圆形碰撞体组件
    private Player player; // 玩家对象

    // 剑的状态标志
    private bool canRotate = true; // 是否可以旋转
    private bool isReturning; // 是否正在返回

    // 冻结时间和返回速度
    private float freezeTimeDuration; // 冻结时间持续时间
    private float returnSpeed = 12; // 返回速度

    // 弹跳相关信息
    [Header("Bounce info")]
    private float bounceSpeed; // 弹跳速度
    private bool isBouncing; // 是否正在弹跳
    private int bounceAmount; // 弹跳次数
    private List<Transform> enemyTarget; // 敌人目标列表
    private int targetIndex = 0; // 当前目标索引

    // 穿透相关信息
    [Header("Pierce info")]
    private float pierceAmount; // 穿透次数

    // 旋转相关信息
    [Header("Spin info")]
    private float maxTravelDistance; // 最大移动距离
    private float spinDuration; // 旋转持续时间
    private float spinTimer; // 旋转计时器
    private bool wasStopped; // 是否已停止
    private bool isSpinning; // 是否正在旋转

    private float hitTimer; // 击中计时器
    private float hitCooldown; // 击中冷却时间

    private float spinDirection; // 旋转方向

    // 在对象被创建时初始化组件
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>(); // 获取动画组件
        rb = GetComponent<Rigidbody2D>(); // 获取刚体组件
        cd = GetComponent<CircleCollider2D>(); // 获取圆形碰撞体组件
    }

    // 销毁对象的方法
    private void DestroyMe()
    {
        Destroy(gameObject); // 销毁当前对象
    }

    // 设置剑的初始状态和行为
    public void SetupSword(Vector2 _dir, float _gravityScale, float _freezeTimeDuration, float _returnSpeed)
    {
        freezeTimeDuration = _freezeTimeDuration; // 设置冻结时间
        returnSpeed = _returnSpeed; // 设置返回速度
        rb.velocity = _dir; // 设置剑的初始速度
        rb.gravityScale = _gravityScale; // 设置重力影响
        player = PlayerManager.instance.player; // 获取玩家对象
        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true); // 开始旋转动画
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1); // 设置旋转方向

        Invoke("DestroyMe", 7); // 超过7秒后销毁对象，防止生成过多
    }

    // 设置弹跳行为
    public void SetupBounce(bool _isBouncing, int _bounceAmount, float _bounceSpeed)
    {
        isBouncing = _isBouncing; // 设置是否弹跳
        bounceAmount = _bounceAmount; // 设置弹跳次数
        bounceSpeed = _bounceSpeed; // 设置弹跳速度

        enemyTarget = new List<Transform>(); // 初始化敌人目标列表
    }

    // 设置穿透行为
    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount; // 设置穿透次数
    }

    // 设置旋转行为
    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning; // 设置是否旋转
        maxTravelDistance = _maxTravelDistance; // 设置最大移动距离
        spinDuration = _spinDuration; // 设置旋转持续时间
        hitCooldown = _hitCooldown; // 设置击中冷却时间
    }

    // 返回剑的方法
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // 冻结所有物理约束
        transform.parent = null; // 解除与任何父对象的关联
        isReturning = true; // 设置为返回状态
    }

    // 每帧更新一次，用于更新剑的状态
    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity; // 根据速度方向旋转剑
        }

        if (isReturning)
        {
            // 如果正在返回，移动到玩家位置
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            // 如果距离玩家小于1，捕捉剑
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword(); // 玩家捕捉剑
            }
        }

        BounceLogic(); // 处理弹跳逻辑
        SpinLogic(); // 处理旋转逻辑
    }

    // 旋转逻辑
    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning(); // 超过最大距离时停止旋转
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime; // 减少旋转计时器
                hitTimer -= Time.deltaTime; // 减少击中计时器

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime); // 移动剑

                if (spinTimer < 0)
                {
                    isReturning = true; // 设置为返回状态
                    isSpinning = false; // 停止旋转
                }
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown; // 重置击中计时器

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1); // 获取周围的碰撞体

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            SwordSkillDamage(hit.GetComponent<Enemy>()); // 造成伤害
                        }
                    }
                }
            }
        }
    }

    // 停止旋转
    private void StopWhenSpinning()
    {
        wasStopped = true; // 设置为已停止
        rb.constraints = RigidbodyConstraints2D.FreezePosition; // 冻结位置
        spinTimer = spinDuration; // 设置旋转持续时间
    }

    // 弹跳逻辑
    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime); // 移动到目标位置
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>()); // 造成伤害

                ++targetIndex; // 移动到下一个目标
                bounceAmount--; // 减少弹跳次数

                if (bounceAmount == 0)
                {
                    isBouncing = false; // 停止弹跳
                    isReturning = true; // 设置为返回状态
                }

                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0; // 重置目标索引
                }
            }
        }
    }

    // 当触发碰撞时调用，用于处理碰撞逻辑
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 如果正在返回，不执行任何操作
        if (isReturning)
        {
            return;
        }

        if (collision.GetComponent<Enemy>() != null)
        {
            SwordSkillDamage(collision.GetComponent<Enemy>()); // 造成伤害
        }

        SetupTargetForBounce(collision); // 设置弹跳目标

        StuckInto(collision); // 处理是否卡住
    }

    // 造成伤害的方法
    private void SwordSkillDamage(Enemy enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>(); // 获取敌人属性
        player.stats.DoDamage(enemyStats); // 造成伤害
        if (player.skill.sword.timeStopUnlocked)
        {
            enemy.FreezeTimeFor(freezeTimeDuration); // 使敌人冻结
        }
        if (player.skill.sword.vulnerableUnlocked)
        {
            enemyStats.MakeVulnerableFor(freezeTimeDuration); // 使敌人脆弱
        }

        ItemData_Equipment equipdAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet); // 获取装备的护身符
        if (equipdAmulet != null)
        {
            equipdAmulet.Effect(enemy.transform); // 触发护身符效果
        }
    }

    // 设置弹跳目标
    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10); // 获取周围的碰撞体

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform); // 添加到目标列表
                    }
                }
            }
        }
    }

    // 处理是否卡住
    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            --pierceAmount; // 减少穿透次数
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning(); // 停止旋转
            return;
        }

        canRotate = false; // 禁止旋转
        cd.enabled = false; // 禁用碰撞体

        rb.isKinematic = true; // 设置为运动学模式
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // 冻结所有物理约束

        GetComponentInChildren<ParticleSystem>().Play(); // 播放粒子效果

        if (isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false); // 停止旋转动画
        transform.parent = collision.transform; // 将剑设置为碰撞对象的子对象
    }
}