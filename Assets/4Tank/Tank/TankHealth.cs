using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_startingHealth = 100.0f;
    public Slider m_slider;
    public Image m_fillImage;
    public Color m_fullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    public GameObject m_explosionPrefab;

    private AudioSource m_explosionAudio;
    private ParticleSystem m_explosionParticles;
    private float m_currentHealth;
    private bool m_dead;

    private void Awake()
    {
        m_explosionParticles = Instantiate(m_explosionPrefab).GetComponent<ParticleSystem>();
        m_explosionAudio = m_explosionParticles.GetComponent<AudioSource>();

        m_explosionParticles.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        m_currentHealth = m_startingHealth;
        m_dead = false;

        SetHealthUI();
    }

    public void TakeDamage(float amount)
    {
        m_currentHealth -= amount;
        SetHealthUI();

        if (m_currentHealth <= 0.0f && !m_dead)
        {
            OnDeath();
        }
    }

    private void SetHealthUI()
    {
        m_slider.value = m_currentHealth;
        m_fillImage.color = Color.Lerp(m_ZeroHealthColor, m_fullHealthColor, m_currentHealth / m_startingHealth);
    }

    private void OnDeath()
    {
        m_dead = true;

        m_explosionParticles.transform.position = transform.position;
        m_explosionParticles.gameObject.SetActive(true);

        m_explosionParticles.Play();

        m_explosionAudio.Play();

        gameObject.SetActive(false);
    }
}