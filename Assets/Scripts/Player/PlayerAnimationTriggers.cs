using System.Collections; // 引入系统集合命名空间
using System.Collections.Generic; // 引入系统泛型集合命名空间
using UnityEngine; // 引入Unity引擎命名空间

// PlayerAnimationTriggers类继承自Unity的MonoBehaviour类，用于处理玩家动画的触发器
public class PlayerAnimationTriggers : MonoBehaviour
{
    // 使用表达式体成员语法获取父对象中的Player组件
    // 这是一个只读属性，用于获取Player组件的引用
    private Player player => GetComponentInParent<Player>();

    // AnimationTrigger方法用于在特定动画事件触发时调用
    // 该方法调用Player类中的AnimationTrigger方法，以同步动画事件与游戏逻辑
    private void AnimationTrigger()
    {
        // 调用Player类的AnimationTrigger方法，传递动画事件信号
        player.AnimationTrigger();
    }
    // AttackTrigger方法用于在攻击动画帧触发时调用

    private void AttackTrigger()
    {
        // 使用Physics2D.OverlapCircleAll方法检测攻击范围内的所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        // 播放攻击音效
        AudioManager.instance.PlaySFX(2, null);
        // 遍历所有检测到的碰撞体
        foreach (var hit in colliders)
        {
            // 检查碰撞体是否为敌人
            if (hit.GetComponent<Enemy>() != null)
            {
                // 获取敌人的状态组件
                EnemyStats target = hit.GetComponent<EnemyStats>();
                // 如果敌人状态组件不为空，执行伤害逻辑
                if(target != null) 
                    player.stats.DoDamage(target);

                // 应用武器的效果
                Inventory.instance.GetEquipment(EquipmentType.Weapon) ?.Effect(target.transform);

            }
        }
    }

    // ThrowSword方法用于在投掷剑的动画事件触发时调用
    private void ThrowSword()
    {
        // 调用SkillManager中的剑术技能，创建剑对象
        SkillManager.instance.sword.CreateSword();
    }
}