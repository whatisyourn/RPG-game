using System.Collections; // 引入System.Collections命名空间
using System.Collections.Generic; // 引入System.Collections.Generic命名空间
using UnityEngine; // 引入UnityEngine命名空间

// 使用CreateAssetMenu属性创建一个可在Unity编辑器中创建的雷击效果脚本对象
[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item effect/Thunder strike")]
public class ThunderStrike_Effect : ItemEffect
{
    // 使用SerializeField属性使私有变量在Unity编辑器中可见，定义雷击预制体
    [SerializeField] private GameObject thunderStrikePrefab;

    // 重写基类的ExecuteEffect方法，执行雷击效果
    // 该方法在敌人位置生成一个雷击预制体，并在0.5秒后销毁
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        // 实例化一个新的雷击预制体，位置为_enemyPosition，旋转为默认值
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity );
        // 在0.5秒后销毁该对象
        Destroy( newThunderStrike, 0.5f);
    }
}
