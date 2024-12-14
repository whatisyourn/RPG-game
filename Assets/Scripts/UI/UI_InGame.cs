// 引入TextMeshPro命名空间，用于处理文本
using TMPro;
// 引入Unity引擎命名空间
using UnityEngine;
// 引入Unity UI命名空间，用于处理UI元素
using UnityEngine.UI;

// 定义UI_InGame类，继承自MonoBehaviour
public class UI_InGame : MonoBehaviour
{
    // 序列化字段，用于在编辑器中设置PlayerStats对象
    [SerializeField] private PlayerStats playerStats;
    // 序列化字段，用于在编辑器中设置Slider对象
    [SerializeField] private Slider slider;

    // 序列化字段，用于在编辑器中设置冲刺技能的图标
    [SerializeField] private Image dashImage;
    // 序列化字段，用于在编辑器中设置格挡技能的图标
    [SerializeField] private Image parryImage;
    // 序列化字段，用于在编辑器中设置水晶技能的图标
    [SerializeField] private Image crystalImage;
    // 序列化字段，用于在编辑器中设置剑技能的图标
    [SerializeField] private Image swordImage;
    // 序列化字段，用于在编辑器中设置黑洞技能的图标
    [SerializeField] private Image blackholeImage;
    // 序列化字段，用于在编辑器中设置药水技能的图标
    [SerializeField] private Image flaskImage;

    // 定义SkillManager类型的私有变量skills
    private SkillManager skills;

    // 在编辑器中显示“Spuls info”标题
    [Header("Spuls info")]
    // 序列化字段，用于在编辑器中设置当前灵魂数量的TextMeshProUGUI组件
    [SerializeField] private TextMeshProUGUI currentSouls;
    // 序列化字段，用于在编辑器中设置灵魂数量
    [SerializeField] private float soulsAmount;
    // 序列化字段，用于在编辑器中设置灵魂增加速率
    [SerializeField] private float increaseRate = 100;

    // Start方法在对象创建时调用
    void Start()
    {
        // 如果playerStats不为空，则为其onHealthChanged事件添加UpdateHealthUI方法
        if (playerStats != null)
        {
            playerStats.onHealthChanged += UpdateHealthUI;
        }

        // 获取SkillManager的实例并赋值给skills变量
        skills = SkillManager.instance;
    }

    // Update方法每帧调用一次
    void Update()
    {
        // 更新灵魂UI
        UpdateSoulsUI();

        // 检查按键输入并设置相应技能的冷却
        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked)
        {
            SetCooldownOf(dashImage);
        }
        if (Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked)
        {
            SetCooldownOf(parryImage);
        }
        if (Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
        {
            SetCooldownOf(crystalImage);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.swordUnlocked)
        {
            SetCooldownOf(swordImage);
        }
        if (Input.GetKeyDown(KeyCode.R) && skills.blackhole.blackholeUnlocked)
        {
            SetCooldownOf(blackholeImage);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
        {
            SetCooldownOf(flaskImage);
        }

        // 检查每个技能的冷却状态
        CheckCooldownOf(dashImage, skills.dash.cooldown);
        CheckCooldownOf(parryImage, skills.parry.cooldown);
        CheckCooldownOf(crystalImage, skills.crystal.cooldown);
        CheckCooldownOf(swordImage, skills.sword.cooldown);
        CheckCooldownOf(blackholeImage, skills.blackhole.cooldown);
        CheckCooldownOf(flaskImage, Inventory.instance.flaskCooldown);
    }

    // 更新灵魂UI的方法
    private void UpdateSoulsUI()
    {
        // 如果当前灵魂数量小于玩家的货币数量，则增加灵魂数量
        if (soulsAmount < PlayerManager.instance.GetCurrency())
        {
            soulsAmount += Time.deltaTime * increaseRate;
        }
        else
        {
            // 否则将灵魂数量设置为玩家的货币数量
            soulsAmount = PlayerManager.instance.GetCurrency();
        }

        // 更新当前灵魂数量的文本显示
        currentSouls.text = ((int)soulsAmount).ToString();
    }

    // 更新血量UI的方法
    private void UpdateHealthUI()
    {
        // 设置血条的最大值为玩家的最大血量
        slider.maxValue = playerStats.GetMaxHealthValue();
        // 设置血条的当前值为玩家的当前血量
        slider.value = playerStats.currentHealth;
    }

    // 设置技能图标冷却的方法
    private void SetCooldownOf(Image _image)
    {
        // 如果图标的填充量小于等于0，则将其设置为1
        if (_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }

    // 检查技能图标冷却状态的方法
    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        // 如果图标的填充量大于0，则根据冷却时间减少填充量
        if (_image.fillAmount > 0)
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }
}
