using System.Collections; // 引入System.Collections命名空间
using System.Collections.Generic; // 引入System.Collections.Generic命名空间
using UnityEngine; // 引入UnityEngine命名空间

// 使用CreateAssetMenu属性创建一个可在Unity编辑器中创建的冰与火效果脚本对象
[CreateAssetMenu(fileName = "Ice and Fire effect", menuName = "Data/Item effect/IceAndFire strike")]
public class IceAndFire_Effect : ItemEffect
{
    // 使用SerializeField属性使私有变量在Unity编辑器中可见，定义冰与火预制体
    [SerializeField] private GameObject IceAndFirePrefab;
    // 使用SerializeField属性使私有变量在Unity编辑器中可见，定义x轴速度
    [SerializeField] private float xVelocity;

    // 重写基类的ExecuteEffect方法，执行冰与火效果
    // 该方法在玩家进行第三次攻击时生成一个冰与火的预制体，并赋予其速度，最后在10秒后销毁
    public override void ExecuteEffect(Transform _respawnPosition)
    {
        // 获取玩家对象
        Player player = PlayerManager.instance.player;
        // 判断玩家是否进行第三次攻击
        bool thirdAttack = player.primaryAttack.comboCounter == 2;
        // 如果是第三次攻击
        if (thirdAttack)
        {
            // 实例化一个新的冰与火预制体，位置为_respawnPosition，旋转与玩家相同
            GameObject newIceAndFiree = Instantiate(IceAndFirePrefab, _respawnPosition.position, player.transform.rotation);
            // 设置新实例化对象的速度，方向根据玩家的朝向
            newIceAndFiree.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);
            // 在10秒后销毁该对象
            Destroy(newIceAndFiree, 10);
        }
    }
}
