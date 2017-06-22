using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_tankMask;
    public ParticleSystem m_explosionParticles;
    public AudioSource m_explosionAudio;
    public float m_maxDamage = 100.0f;
    public float m_explosionForce = 1000.0f;
    public float m_MaxLifeTime = 2.0f;
    public float m_explosionRadius = 5.0f;

    private void Start()
    {
        // 炮弹存活到最长时间后销毁它
        Destroy(gameObject, m_MaxLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var colliders = Physics.OverlapSphere(transform.position, m_explosionRadius, m_tankMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            var targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            if (!targetRigidbody)
            {
                continue;
            }
            targetRigidbody.AddExplosionForce(m_explosionForce, transform.position, m_explosionRadius);

            // todo: 坦克伤害
        }

        m_explosionParticles.transform.parent = null;
        m_explosionParticles.Play();
        m_explosionAudio.Play();

        var mainModule = m_explosionParticles.main;
        Destroy(m_explosionParticles.gameObject, mainModule.duration);

        Destroy(gameObject);
    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        var explosionToTarget = targetPosition - transform.position;
        var explosionDistance = explosionToTarget.magnitude;
        var relativeDistance = (m_explosionRadius - explosionDistance) / m_explosionRadius;
        var damage = relativeDistance * m_maxDamage;
        damage = Mathf.Max(0.0f, damage);

        return damage;
    }
}