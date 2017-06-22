using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    public bool m_useRelativeRotation = true;

    private Quaternion m_relativeRotation;

    private void Start()
    {
        m_relativeRotation = transform.parent.localRotation;
    }

    private void Update()
    {
        if (m_useRelativeRotation)
        {
            transform.rotation = m_relativeRotation;
        }
    }
}