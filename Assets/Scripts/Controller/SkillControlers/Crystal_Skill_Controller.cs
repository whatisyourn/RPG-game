using UnityEngine;

// 水晶技能控制器类，继承自MonoBehaviour
public class Crystal_Skill_Controller : MonoBehaviour
{
    // 获取Animator组件，用于控制动画
    private Animator anim => GetComponent<Animator>();
    // 获取CircleCollider2D组件，用于检测碰撞
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    // 水晶存在的计时器
    private float crystalExistTimer;

    // 是否可以爆炸
    private bool canExplode;
    // 是否可以移动
    private bool canMove;
    // 移动速度
    private float moveSpeed;

    // 是否可以增长
    private bool canGrow;
    // 增长速度
    private float growSpeed = 5;

    // 最近的目标
    private Transform closestTarget;
    // 用于检测敌人的图层掩码
    [SerializeField] private LayerMask whatIsEnemy;

    // 设置水晶的初始状态
    public void SetupCrystal(float _crystalDuration, bool _canExplosion, bool _canMove, float _moveSpeed, Transform _closestTarget)
    {
        crystalExistTimer = _crystalDuration; // 设置水晶存在的时间
        canExplode = _canExplosion; // 设置是否可以爆炸
        canMove = _canMove; // 设置是否可以移动
        moveSpeed = _moveSpeed; // 设置移动速度
        closestTarget = _closestTarget; // 设置最近的目标
    }

    // 随机选择一个敌人作为目标
    public void ChooseRandomEnemy()
    {
        // 获取黑洞的半径
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();
        // 获取在指定半径范围内的所有敌人碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        // 如果有敌人
        if(colliders.Length > 0 )
        {
            // 随机选择一个敌人作为最近的目标
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }

    // 每帧更新水晶状态
    private void Update()
    {
        crystalExistTimer -= Time.deltaTime; // 减少水晶存在的时间

        if (crystalExistTimer < 0) // 如果水晶存在时间小于0
        {
            FinishCrystal(); // 完成水晶技能
        }
        // 如果可以移动并且有最近的目标
        if (canMove && closestTarget != null)
        {
            // 移动到最近的目标
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestTarget.position) < 1) // 如果距离小于1
            {
                FinishCrystal(); // 完成水晶技能
                canMove = false; // 爆炸时应停止移动
            }
        }

        if (canGrow) // 如果可以增长
        {
            // 在增长时，碰撞体也会随之增大
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    // 动画爆炸事件
    private void AnimationExplodeEvent()
    {
        // 获取在碰撞体半径范围内的所有敌人碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null) // 如果碰撞体是敌人
            {
                // 设置敌人的击退方向
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                // 对敌人造成魔法伤害
                PlayerManager.instance.player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());

                // 触发装备效果
                ItemData_Equipment equipdAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
                if (equipdAmulet != null) {
                    equipdAmulet.Effect(hit.transform);
                }
            }
        }
    }

    // 完成水晶技能
    public void FinishCrystal()
    {
        if (canExplode) // 如果可以爆炸
        {
            canGrow = true; // 设置可以增长
            anim.SetTrigger("Explode"); // 触发爆炸动画
        }
        else
        {
            SelfDestroy(); // 自我销毁
        }
    }

    // 自我销毁
    public void SelfDestroy() => Destroy(gameObject);
}
