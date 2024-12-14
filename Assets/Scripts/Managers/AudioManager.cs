using UnityEngine; // 引入Unity引擎命名空间
using static Unity.VisualScripting.Member; // 引入VisualScripting的Member类

// 定义一个AudioManager类，继承自MonoBehaviour
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // 定义一个静态实例，用于实现单例模式

    [SerializeField] private float sfxMinimumistance; // 定义一个私有浮点变量，用于存储音效的最小距离
    [SerializeField] private AudioSource[] sfx; // 定义一个私有数组，用于存储音效的音频源
    [SerializeField] private AudioSource[] bgm; // 定义一个私有数组，用于存储背景音乐的音频源

    public bool playBGM; // 定义一个布尔变量，用于控制是否播放背景音乐
    private int bgmIndex; // 定义一个私有整数变量，用于存储当前播放的背景音乐索引

    // Awake函数，用于初始化单例模式
    private void Awake()
    {
        if (instance != null) // 如果实例不为空
            Destroy(instance.gameObject); // 销毁已有的实例对象
        else
            instance = this; // 将当前实例赋值给静态实例
    }

    // Update函数，每帧调用一次，用于更新背景音乐的播放状态
    private void Update()
    {
        if (!playBGM) // 如果不播放背景音乐
        {
            StopAllBGM(); // 停止所有背景音乐
        }
        else
        {
            if (!bgm[bgmIndex].isPlaying) // 如果当前背景音乐没有播放
            {
                PlayBGM(bgmIndex); // 播放当前索引的背景音乐
            }
        }
    }

    // PlaySFX函数，用于播放指定索引的音效
    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        // 如果音效已经在播放，则不再播放
        if (sfx[_sfxIndex].isPlaying)
        {
            return; // 直接返回
        }
        // 如果音效源距离玩家过远，则不播放
        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumistance)
        {
            return; // 直接返回
        }

        if (_sfxIndex < sfx.Length) // 如果索引在音效数组范围内
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f); // 随机调整音效的音调，使其不完全相同
            sfx[_sfxIndex].Play(); // 播放音效
        }
    }

    // StopSFX函数，用于停止指定索引的音效
    public void StopSFX(int _sfxIndex) => sfx[_sfxIndex].Stop(); // 停止音效

    // PlayBGM函数，用于播放指定索引的背景音乐
    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex; // 设置当前背景音乐索引
        StopAllBGM(); // 停止所有背景音乐
        bgm[bgmIndex].Play(); // 播放当前索引的背景音乐
    }

    // PlayRandomBGM函数，用于随机播放背景音乐
    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length); // 随机选择一个背景音乐索引
        PlayBGM(bgmIndex); // 播放随机选择的背景音乐
    }

    // StopAllBGM函数，用于停止所有背景音乐
    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++) // 遍历所有背景音乐
        {
            bgm[i].Stop(); // 停止每一个背景音乐
        }
    }
}
