using System.Collections.Generic;
using UnityEngine;

// 黑洞技能控制器类，继承自MonoBehaviour
public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab; // 热键预制体
    [SerializeField] private List<KeyCode> keyCodeList; // 热键列表

    private float maxSize; // 最大尺寸
    private float growSpeed; // 增长速度
    private float shrinkSpeed; // 缩小速度
    private float blackholeTimer; // 黑洞计时器

    private bool canGrow = true; // 是否可以增长
    private bool canShrink; // 是否可以缩小
    private bool canCreateHotKey = true; // 是否可以创建热键
    private bool cloneAttackRelease; // 是否释放克隆攻击
    private bool playerCanDisappear = true; // 玩家是否可以消失

    private int amountOfAttacks = 4; // 攻击次数
    private float cloneAttackCooldown = .3f; // 克隆攻击冷却时间
    private float cloneAttackTimer; // 克隆攻击计时器

    private List<Transform> targets = new List<Transform>(); // 目标列表
    private List<GameObject> createHotKey = new List<GameObject>(); // 创建的热键列表

    public bool playerCanExitState { get; private set; } // 玩家是否可以退出状态

    // 设置黑洞技能参数
    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize; // 设置最大尺寸
        growSpeed = _growSpeed; // 设置增长速度
        shrinkSpeed = _shrinkSpeed; // 设置缩小速度
        amountOfAttacks = _amountOfAttacks; // 设置攻击次数
        cloneAttackCooldown = _cloneAttackCooldown; // 设置克隆攻击冷却时间
        blackholeTimer = _blackholeDuration; // 设置黑洞持续时间
    }

    // 每帧更新黑洞技能状态
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime; // 减少克隆攻击计时器
        blackholeTimer -= Time.deltaTime; // 减少黑洞计时器

        if (blackholeTimer < 0) // 如果黑洞计时器小于0
        {
            blackholeTimer = Mathf.Infinity; // 确保只执行一次
            if (targets.Count > 0) // 如果有目标
            {
                ReleaseCloneAttack(); // 释放克隆攻击
            }
            else
            {
                FinishBlackHoleAbility(); // 完成黑洞技能
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) // 如果按下R键
        {
            ReleaseCloneAttack(); // 释放克隆攻击
        }
        CloneAttackLogic(); // 执行克隆攻击逻辑

        if (canGrow && !canShrink) // 如果可以增长且不能缩小
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime); // 增长黑洞
        }
        // 开始缩小黑洞
        if (canShrink) // 如果可以缩小
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime); // 缩小黑洞

            // 缩小完成
            if (transform.localScale.x < 0) // 如果黑洞尺寸小于0
            {
                Destroy(gameObject); // 销毁黑洞对象
            }
        }
        if (SkillManager.instance.clone.crystalInstalOfClone) // 如果克隆技能替代为水晶技能
        {
            playerCanDisappear = false; // 玩家不能消失
        }
    }

    // 释放克隆攻击
    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0) // 如果没有目标
        {
            return; // 返回
        }

        DestroyHotKeys(); // 销毁热键
        cloneAttackRelease = true; // 设置克隆攻击释放为真
        canCreateHotKey = false; // 设置不能创建热键
        if (playerCanDisappear) // 如果玩家可以消失
        {
            PlayerManager.instance.player.fx.MakeTransparent(true); // 使玩家透明
            playerCanDisappear = false; // 设置玩家不能消失
        }
    }

    // 克隆攻击逻辑
    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackRelease && amountOfAttacks > 0) // 如果克隆攻击计时器小于0且克隆攻击释放且攻击次数大于0
        {
            cloneAttackTimer = cloneAttackCooldown; // 重置克隆攻击计时器

            int randomIndex = Random.Range(0, targets.Count); // 随机选择目标索引

            float xOffset; // x轴偏移量

            if (Random.Range(0, 100) > 50) // 随机选择偏移方向
            {
                xOffset = 2; // 设置偏移量为2
            }
            else
            {
                xOffset = -2; // 设置偏移量为-2
            }

            if (SkillManager.instance.clone.crystalInstalOfClone) // 如果克隆技能替代为水晶技能
            {
                SkillManager.instance.crystal.CreateCrystal(); // 创建水晶
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget(); // 水晶选择随机目标
            }
            else
            {
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0)); // 创建克隆
            }
            amountOfAttacks--; // 减少攻击次数
            if (amountOfAttacks <= 0) // 如果攻击次数小于等于0
            {
                Invoke("FinishBlackHoleAbility", .5f); // 延迟0.5秒完成黑洞技能
            }
        }
    }

    // 完成黑洞技能
    private void FinishBlackHoleAbility()
    {
        DestroyHotKeys(); // 销毁热键
        playerCanExitState = true; // 设置玩家可以退出状态
        canShrink = true; // 设置可以缩小
        cloneAttackRelease = false; // 设置克隆攻击释放为假
    }

    // 销毁热键
    private void DestroyHotKeys()
    {
        if (createHotKey.Count < 0) // 如果创建的热键数量小于0
        {
            return; // 返回
        }

        for (int i = 0; i < createHotKey.Count; i++) // 遍历创建的热键列表
        {
            Destroy(createHotKey[i]); // 销毁热键
        }
    }

    // 当触发碰撞时，冻结敌人并创建热键
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null) // 如果碰撞对象是敌人
        {
            collision.GetComponent<Enemy>().FreezeTime(true); // 冻结敌人

            CreateHotKey(collision); // 创建热键
        }
    }

    // 当退出碰撞时，解除敌人的冻结状态
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null) // 如果碰撞对象是敌人
        {
            collision.GetComponent<Enemy>().FreezeTime(false); // 解除敌人冻结状态
        }
    }

    // 创建热键
    private void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0) // 如果热键列表为空
        {
            Debug.LogWarning("Not enough"); // 输出警告信息
            return; // 返回
        }

        // 防止在使用技能后创建新的热键
        if (!canCreateHotKey) // 如果不能创建热键
            return; // 返回

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity); // 实例化新的热键
        createHotKey.Add(newHotKey); // 将新热键添加到列表中

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)]; // 随机选择一个热键
        keyCodeList.Remove(choosenKey); // 从热键列表中移除选择的热键

        Blackhole_HotKey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_HotKey_Controller>(); // 获取新热键的控制脚本
        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this); // 设置热键
    }

    // 将敌人添加到目标列表
    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
