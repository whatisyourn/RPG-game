using UnityEngine; // 引入Unity引擎命名空间
using UnityEngine.UI; // 引入Unity UI命名空间

// ���彣������ö��
public enum SwordType
{
    Regular, // ��ͨ��
    Bounce, // ������
    Pierce, // ��͸��
    Spin // ��ת��
}

// ���彣�����࣬�̳���Skill��
public class Sword_Skill : Skill
{
    // ���彣������
    public SwordType swordType = SwordType.Regular; // 设定默认的剑类型为普通剑

    // ����������Ϣ
    [Header("Bounce info")]
    [SerializeField] private UI_SkillTreeSlot bounceUnlockButton; // 解锁弹跳剑的按钮
    [SerializeField] private int bounceAmount; // 弹跳次数
    [SerializeField] private float bounceGravity; // 弹跳重力
    [SerializeField] private float bounceSpeed; // 弹跳速度

    // ��͸������Ϣ
    [Header("Pierce info")]
    [SerializeField] private UI_SkillTreeSlot pierceUnlockButton; // 解锁穿透剑的按钮
    [SerializeField] private int pierceAmount; // 穿透次数
    [SerializeField] private float pierceGravity; // 穿透重力

    // ��ת������Ϣ
    [Header("Spin info")]
    [SerializeField] private UI_SkillTreeSlot spinUnlockButton; // 解锁旋转剑的按钮
    [SerializeField] private float hitCooldown = .35f; // 击中冷却时间
    [SerializeField] private float maxTravelDistance = 7; // 最大移动距离
    [SerializeField] private float spinDuration = 2; // 旋转持续时间
    [SerializeField] private float spinGravity = 1; // 旋转重力

    // ������Ϣ
    [Header("Skill info")]
    [SerializeField] private UI_SkillTreeSlot swordUnlockButton; // 解锁剑的按钮
    public bool swordUnlocked { get; private set; } // 剑是否解锁的标志
    [SerializeField] private GameObject swordPrefab; // 剑的预制体
    [SerializeField] private Vector2 launchForce; // 发射力
    [SerializeField] private float swordGravity; // 剑的重力
    [SerializeField] private float freezeTimeDuration; // 冻结时间
    [SerializeField] private float returnSpeed; // 返回速度

    [Header("Passive skills")]
    [SerializeField] private UI_SkillTreeSlot timeStopUnlockButton; // 解锁时间停止的按钮
    public bool timeStopUnlocked { get; private set; } // 时间停止是否解锁的标志
    [SerializeField] private UI_SkillTreeSlot vulnerableUnlockButton; // 解锁脆弱状态的按钮
    public bool vulnerableUnlocked { get; private set; } // 脆弱状态是否解锁的标志

    private Vector2 finalDir; // 最终方向

    // ��׼�����Ϣ
    [Header("Aim dots")]
    [SerializeField] private int numberOfDots; // 瞄准点的数量
    [SerializeField] private float spaceBetweenDots; // 瞄准点之间的距离
    [SerializeField] private GameObject dotPrefab; // 瞄准点的预制体
    [SerializeField] private Transform dotsParents; // 瞄准点的父对象

    private GameObject[] dots; // 瞄准点数组

    // ���������������ʱ�����û����Start��������������׼��
    protected override void Start()
    {
        base.Start(); // 调用基类的Start方法
        GenerateDots(); // 生成瞄准点

        // 为各个解锁按钮添加监听事件
        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounceSword);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnurable);
    }

    // ���ݽ����������ý�������
    protected void SetupGravity()
    {
        switch (swordType) // 根据剑的类型设置重力
        {
            case SwordType.Bounce: // 如果是弹跳剑
                swordGravity = bounceGravity; // 设置弹跳重力
                break;
            case SwordType.Pierce: // 如果是穿透剑
                swordGravity = pierceGravity; // 设置穿透重力
                break;
            case SwordType.Spin: // 如果是旋转剑
                swordGravity = spinGravity; // 设置旋转重力
                break;
            default: // 默认情况下不改变重力
                break;
        }
    }

    // ÿ֡���õ�Update�����������������׼���λ�ø���
    protected override void Update()
    {
        base.Update(); // 调用基类的Update方法
        SetupGravity(); // 设置重力

        // 如果鼠标右键松开，计算瞄准方向并设置最终方向
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Vector2 dir = AimDirection(); // 获取瞄准方向
            finalDir = new Vector2(dir.normalized.x * launchForce.x, dir.normalized.y * launchForce.y); // 设置最终方向
        }

        // 如果鼠标右键按住，更新瞄准点的位置
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPostion(i * spaceBetweenDots); // 更新每个瞄准点的位置
            }
        }
    }

    // ��������ʵ���������ݽ����������ò�ͬ����Ϊ
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation); // 实例化新的剑
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>(); // 获取新剑的控制脚本

        // 根据剑的类型设置不同的行为
        switch (swordType)
        {
            case SwordType.Bounce:
                newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed); // 设置弹跳行为
                break;
            case SwordType.Pierce:
                newSwordScript.SetupPierce(pierceAmount); // 设置穿透行为
                break;
            case SwordType.Spin:
                newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown); // 设置旋转行为
                break;
            default:
                break;
        }

        // 设置剑的物理属性，并将其分配给玩家
        newSwordScript.SetupSword(finalDir, swordGravity, freezeTimeDuration, returnSpeed);
        player.AssignNewSword(newSword); // 将新剑分配给玩家
        DotsActivate(false); // 禁用瞄准点
    }

    #region Unlock region

    // 检查并解锁所有技能
    protected override void CheckUnlock()
    {
        UnlockSword(); // 解锁普通剑
        UnlockBounceSword(); // 解锁弹跳剑
        UnlockSpinSword(); // 解锁旋转剑
        UnlockPierceSword(); // 解锁穿透剑
        UnlockTimeStop(); // 解锁时间停止
        UnlockVulnurable(); // 解锁脆弱状态
    }

    // 解锁时间停止技能
    private void UnlockTimeStop()
    {
        if (timeStopUnlockButton.unlocked) // 如果时间停止按钮已解锁
            timeStopUnlocked = true; // 设置时间停止已解锁
    }

    // 解锁脆弱状态技能
    private void UnlockVulnurable()
    {
        if (vulnerableUnlockButton.unlocked) // 如果脆弱状态按钮已解锁
            vulnerableUnlocked = true; // 设置脆弱状态已解锁
    }

    // 解锁普通剑技能
    private void UnlockSword()
    {
        if (swordUnlockButton.unlocked) // 如果普通剑按钮已解锁
        {
            swordUnlocked = true; // 设置普通剑已解锁
            swordType = SwordType.Regular; // 设置剑类型为普通剑
        }
    }

    // 解锁弹跳剑技能
    private void UnlockBounceSword()
    {
        if (bounceUnlockButton.unlocked) // 如果弹跳剑按钮已解锁
            swordType = SwordType.Bounce; // 设置剑类型为弹跳剑
    }

    // 解锁穿透剑技能
    private void UnlockPierceSword()
    {
        if (pierceUnlockButton.unlocked) // 如果穿透剑按钮已解锁
            swordType = SwordType.Pierce; // 设置剑类型为穿透剑
    }

    // 解锁旋转剑技能
    private void UnlockSpinSword()
    {
        if (spinUnlockButton.unlocked) // 如果旋转剑按钮已解锁
            swordType = SwordType.Spin; // 设置剑类型为旋转剑
    }
    #endregion

    #region Aim

    // 获取瞄准方向
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position; // 获取玩家位置
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 获取鼠标位置（世界坐标）
        Vector2 direction = mousePosition - playerPosition; // 计算方向
        return direction; // 返回方向
    }

    // 激活或禁用瞄准点
    public void DotsActivate(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive); // 设置每个瞄准点的激活状态
        }
    }

    // 生成瞄准点
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots]; // 初始化瞄准点数组
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParents); // 实例化瞄准点
            dots[i].SetActive(false); // 默认禁用瞄准点
        }
    }

    // 计算瞄准点的位置
    private Vector2 DotsPostion(float t)
    {
        // 使用抛物线运动公式计算水平和垂直方向的偏移
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x, // 计算水平方向的偏移
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t); // 计算垂直方向的偏移
        return position; // 返回计算后的位置
    }

    #endregion
}