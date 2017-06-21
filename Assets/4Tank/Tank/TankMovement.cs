using UnityEngine;

public class TankMovement : MonoBehaviour
{
    // 区分哪个坦克属于哪个玩家，这个由 TankManager 设置
    public int m_playerNumber = 1;

    // 前进后退速度
    public float m_speed = 12.0f;
    // 转向速度
    public float m_turnSpeed = 180.0f;

    // 坦克前进后退转向时播放声音的音源对象
    public AudioSource m_moveMentAudio;
    // 坦克静止时的音效
    public AudioClip m_engineIdling;
    // 坦克运动时的音效
    public AudioClip m_engineDriving;
    // 音效音高范围，初始 - m_pitchRange ~ 初始 + m_pitchRange
    public float m_pitchRange = 0.2f;

    // 这个玩家在输入系统里边运动轴的名字，这是 TankMovement 自己拼出来的
    private string m_movementAxisName;
    // 这个玩家在输入系统里边转向轴的名字，这是 TankMovement 自己拼出来的
    private string m_turnAxisName;
    // 坦克对象的刚体对象
    private Rigidbody m_rigidbody;
    // 运动轴的输入值
    private float m_movementInputValue;
    // 转向轴的输入值
    private float m_turnInputValue;
    // 坦克运动时的播放声音的初始音高
    private float m_originalPitch;
    // 坦克身上的粒子效果
    private ParticleSystem[] m_particleSystems;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        m_rigidbody.isKinematic = false;

        m_movementInputValue = 0.0f;
        m_turnInputValue = 0.0f;

        m_particleSystems = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < m_particleSystems.Length; i++)
        {
            m_particleSystems[i].Play();
        }
    }

    private void OnDisable()
    {
        m_rigidbody.isKinematic = true;

        for (int i = 0; i < m_particleSystems.Length; i++)
        {
            m_particleSystems[i].Stop();
        }
    }

    private void Start()
    {
        m_movementAxisName = "Vertical" + m_playerNumber;
        m_turnAxisName = "Horizontal" + m_playerNumber;

        m_originalPitch = m_moveMentAudio.pitch;
    }

    private void Update()
    {
        m_movementInputValue = Input.GetAxis(m_movementAxisName);
        m_turnInputValue = Input.GetAxis(m_turnAxisName);

        EngineAudio();
    }

    private void EngineAudio()
    {
        // 如果输入很小，我们认为它没有动，切换播放静止的音效
        if (Mathf.Abs(m_movementInputValue) < 0.1f && Mathf.Abs(m_turnInputValue) < 0.1f)
        {
            if (m_moveMentAudio.clip == m_engineDriving)
            {
                m_moveMentAudio.clip = m_engineIdling;
                m_moveMentAudio.pitch = Random.Range(m_originalPitch - m_pitchRange, m_originalPitch + m_pitchRange);
                m_moveMentAudio.Play();
            }
        }
        // 否则，切换播放运动的音效
        else
        {
            if (m_moveMentAudio.clip == m_engineIdling)
            {
                m_moveMentAudio.clip = m_engineDriving;
                m_moveMentAudio.pitch = Random.Range(m_originalPitch - m_pitchRange, m_originalPitch + m_pitchRange);
                m_moveMentAudio.Play();
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        var movement = transform.forward * m_movementInputValue * m_speed * Time.deltaTime;
        m_rigidbody.MovePosition(m_rigidbody.position + movement);
    }

    private void Turn()
    {
        var turn = m_turnInputValue * m_turnSpeed * Time.deltaTime;
        var turnRotation = Quaternion.Euler(0.0f, turn, 0.0f);
        m_rigidbody.MoveRotation(m_rigidbody.rotation * turnRotation);
    }
}