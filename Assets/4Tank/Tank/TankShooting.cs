using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    // 区分属于哪个玩家，这个由 TankManager 设置
    public int m_playerNumber = 1;

    // 炮弹的预置资源
    public Rigidbody m_shellPrefab;
    // 炮弹初始位置
    public Transform m_fireTransform;
    // 显示炮弹当前推动力的滑块 UI
    public Slider m_aimSlider;

    // 坦克开火和蓄力时播放声音的音源对象
    public AudioSource m_shootingAudio;
    // 蓄力的音效
    public AudioClip m_chargingClip;
    // 开火的音效
    public AudioClip m_fireClip;

    // 开火键完全不被长按时给炮弹的最小的推动力
    public float m_minLaunchForce = 15.0f;
    // 开火键被完全长按时给炮弹的最大的推动力
    public float m_maxLaunchForce = 30.0f;
    // 开火键长按增大对炮弹推动力的最大持续时间
    public float m_maxChargeTime = 0.75f;

    // 这个玩家在输入系统里边开火键的名字，这个是 TankShooting 自己拼出来的
    private string m_fireButton;
    // 开火键松开时，给炮弹的当前推动力大小
    private float m_currentLaunchForce;
    // 推动力增加的速度，基于推动力范围和最大的长按持续时间决定
    private float m_chargeSpeed;
    // 是否已经开火了，否则处于蓄力状态
    private bool m_fired;

    private void OnEnable()
    {
        m_currentLaunchForce = m_minLaunchForce;
        m_aimSlider.value = m_minLaunchForce;
    }

    private void Start()
    {
        m_fireButton = "Fire" + m_playerNumber;
        m_chargeSpeed = (m_maxLaunchForce - m_minLaunchForce) / m_maxChargeTime;
    }

    private void Update()
    {
        m_aimSlider.value = m_minLaunchForce;

        // 蓄力状态，并且推动力已经大于最大蓄力了...
        if (m_currentLaunchForce >= m_maxLaunchForce && !m_fired)
        {
            // ...那么我们用最大推动力发射炮弹
            m_currentLaunchForce = m_maxLaunchForce;
            Fire();
        }
        // 否则，如果开火键刚刚被按下...
        else if (Input.GetButtonDown(m_fireButton))
        {
            // ...那么我们设置当前为蓄力初始状态
            m_fired = false;
            m_currentLaunchForce = m_minLaunchForce;

            // 并且播放蓄力音效
            m_shootingAudio.clip = m_chargingClip;
            m_shootingAudio.Play();
        }
        // 否则，如果开火键一直被按住，并且还没有发射炮弹，处于蓄力状态...
        else if (Input.GetButton(m_fireButton) && !m_fired)
        {
            // ...增加炮弹推进力蓄力，更新炮弹当前推进力的滑块 UI
            m_currentLaunchForce += m_chargeSpeed * Time.deltaTime;
            m_aimSlider.value = m_currentLaunchForce;
        }
        // 否则，如果开火键刚刚被放开了，但是还处于蓄力状态...
        else if (Input.GetButtonUp(m_fireButton) && !m_fired)
        {
            // ...那么发射炮弹
            Fire();
        }
    }

    private void Fire()
    {
        m_fired = true;

        var shellInstance = Instantiate(m_shellPrefab, m_fireTransform.position, m_fireTransform.rotation) as Rigidbody;
        shellInstance.velocity = m_currentLaunchForce * m_fireTransform.forward;

        m_shootingAudio.clip = m_fireClip;
        m_shootingAudio.Play();

        m_currentLaunchForce = m_minLaunchForce;
    }
}