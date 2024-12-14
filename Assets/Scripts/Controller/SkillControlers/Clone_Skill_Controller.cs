using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player; // 玩家对象的引用
    private SpriteRenderer sr; // 克隆体的SpriteRenderer组件，用于改变颜色和透明度
    private Animator anim; // 克隆体的Animator组件，用于播放动画

    private float cloneTimer; // 克隆体存在的计时器
    private float attackMultiplier; // 攻击倍率
    [SerializeField] private float colorLoosingSpeed; // 克隆体消失时颜色透明度降低的速度
    [SerializeField] private Transform attackCheck; // 用于检测攻击范围内敌人的Transform
    [SerializeField] private float attackCheckRadius = .8f; // 攻击范围的半径
    private Transform closestEnemy; // 最近的敌人
    private int appearingDir = 1; // 默认克隆体出现在玩家的右侧，负值表示出现在左侧

    private bool canDuplicateClone; // 克隆体是否可以复制出新的克隆体
    private float chanceToDuplicate; // 克隆体复制的概率

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>(); // 获取SpriteRenderer组件
        anim = GetComponent<Animator>(); // 获取Animator组件
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime; // 减少克隆体存在的时间

        if (cloneTimer < 0)
        {
            // 减少颜色透明度，直到克隆体完全透明并销毁
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));
            if (sr.color.a <= 0)
            {
                Destroy(gameObject); // 销毁克隆体
            }
        }
    }

    // 设置克隆体的初始状态
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicateClone, float _chanceToDuplicate, Player _player, float _attackMultiplier)
    {
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 3)); // 随机设置攻击动画
        }

        transform.position = _newTransform.position + _offset; // 设置克隆体的位置
        cloneTimer = _cloneDuration; // 设置克隆体存在的时间
        closestEnemy = _closestEnemy; // 设置最近的敌人
        canDuplicateClone = _canDuplicateClone; // 设置是否可以复制克隆体
        chanceToDuplicate = _chanceToDuplicate; // 设置复制的概率
        FaceClosestTarget(); // 面向最近的敌人
        player = _player; // 设置玩家对象
        attackMultiplier = _attackMultiplier; // 设置攻击倍率
    }

    // 动画事件触发，用于在动画结束后立即销毁克隆体
    private void AnimationTrigger()
    {
        cloneTimer = -.1f; // 设置克隆体的计时器为负值以触发销毁
    }

    // 动画事件触发，用于在攻击帧时对范围内的敌人造成伤害
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius); // 获取攻击范围内的所有碰撞体

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null) // 如果碰撞体是敌人
            {
                // 设置敌人的击退方向
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);

                PlayerStats playerStats = player.GetComponent<PlayerStats>(); // 获取玩家的属性
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>(); // 获取敌人的属性
                playerStats.CloneDoDamage(enemyStats, attackMultiplier); // 对敌人造成伤害

                if (player.skill.clone.canApplyOnHitEffect) // 如果可以应用击中效果
                {
                    Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(hit.transform); // 应用武器效果
                }

                if (canDuplicateClone) // 如果可以复制克隆体
                {
                    if (Random.Range(0, 100) < chanceToDuplicate) // 根据概率复制克隆体
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(0.5f * appearingDir, 0)); // 创建新的克隆体
                    }
                }
            }
        }
    }

    // 使克隆体面向最近的敌人
    private void FaceClosestTarget()
    {
        if (closestEnemy != null)
        {
            // 如果最近的敌人在左侧，则翻转克隆体
            if (transform.position.x > closestEnemy.position.x)
            {
                appearingDir *= -1; // 翻转方向
                transform.Rotate(0, 180, 0); // 翻转克隆体
            }
        }
    }
}