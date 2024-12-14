using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderSrike_Controller : MonoBehaviour
{
    // 当2D碰撞触发时调用此方法
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞对象是否为敌人
        if (collision.GetComponent<Enemy>() != null) {
            // 获取玩家的属性
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            // 获取敌人的属性
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();

            // 对敌人造成魔法伤害
            playerStats.DoMagicalDamage(enemyTarget);
        }
    }
}
