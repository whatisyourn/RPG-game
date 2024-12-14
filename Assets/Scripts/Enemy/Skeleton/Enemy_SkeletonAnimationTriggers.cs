// 引入Unity引擎命名空间，以便使用Unity中的类和方法
using UnityEngine;

// 定义Enemy_SkeletonAnimationTriggers类，继承自MonoBehaviour，用于处理动画触发事件
public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    // 使用GetComponentInParent方法获取父对象中的Enemy_Skeleton组件，用于访问敌人的属性和方法
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    // AnimationTrigger方法：在动画中某个特定时刻调用，用于触发敌人的动画事件
    private void AnimationTrigger()
    {
        // 调用enemy对象的AnimationTrigger方法，执行相应的动画事件
        enemy.AnimationTrigger();
    }

    // AttackTrigger方法：在攻击动画中调用，用于检测攻击范围内的玩家并造成伤害
    private void AttackTrigger()
    {
        // 使用Physics2D.OverlapCircleAll方法，以enemy.attackCheck.position为圆心，enemy.attackCheckRadius为半径，检测范围内的所有Collider2D对象
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        // 遍历检测到的所有Collider2D对象
        foreach (var hit in colliders)
        {
            // 如果检测到的对象是Player类型，则执行以下操作
            if (hit.GetComponent<Player>() != null)
            {
                // 获取Player对象的PlayerStats组件
                PlayerStats target = hit.GetComponent<PlayerStats>();

                // 调用enemy.stats的DoDamage方法，对目标造成伤害
                enemy.stats.DoDamage(target);

                // 调用Player对象的DamageEffect方法，显示受伤效果（注释掉的代码）
                // hit.GetComponent<Player>().DamageEffect();
            }
        }
    }

    // OpenCounterrAttackWindow方法：在动画中调用，用于打开反击窗口
    private void OpenCounterrAttackWindow() => enemy.OpenCounterrAttackWindow();

    // CloseCounterrAttackWindow方法：在动画中调用，用于关闭反击窗口
    private void CloseCounterrAttackWindow() => enemy.CloseCounterrAttackWindow();
}