using UnityEngine;

/// <summary>
/// 操作摄像机的大小和位置以便多个目标都同时出现在屏幕中
/// </summary>
public class CameraControl : MonoBehaviour
{
    // 摄像机大小和位置平滑变化时的阻尼系数
    public float m_dampTime = 0.2f;
    // 计算摄像机大小时，目标与摄像机最外边缘的预留位置大小
    public float m_screenEdgeBuffer = 4.0f;
    // 摄像机的最小大小
    public float m_minSize = 6.5f;
    // 要被摄像机照到的所有目标
    public Transform[] m_targets;

    // 摄像机实例
    private Camera m_camera;
    // 当前摄像机大小改变的速度，会被平滑改变
    private float m_zoomSpeed;
    // 当前摄像机位置改变的速度，会被平滑改变
    private Vector3 m_moveVelocity;
    // 摄像机要移动到的位置
    private Vector3 m_desiredPosition;


    private void Awake()
    {
        m_camera = GetComponentInChildren<Camera>();
    }

    private void FixedUpdate()
    {
        Move();

        Zoom();
    }

    private void Move()
    {
        FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, m_desiredPosition, ref m_moveVelocity, m_dampTime);
    }

    /// <summary>
    /// 找到所有目标的中心点，并且设置它为摄像机要移动到的位置
    /// </summary>
    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < m_targets.Length; i++)
        {
            if (!m_targets[i].gameObject.activeSelf)
            {
                continue;
            }

            averagePos += m_targets[i].position;
            numTargets++;
        }

        if (numTargets > 0)
        {
            averagePos /= numTargets;
        }

        averagePos.y = transform.position.y;
        m_desiredPosition = averagePos;
    }

    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        m_camera.orthographicSize =
            Mathf.SmoothDamp(m_camera.orthographicSize, requiredSize, ref m_zoomSpeed, m_dampTime);
    }

    /// <summary>
    /// 取得能让所有目标进度镜头的摄像机大小
    /// </summary>
    /// <returns></returns>
    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_desiredPosition);

        float size = 0.0f;

        for (int i = 0; i < m_targets.Length; i++)
        {
            if (!m_targets[i].gameObject.activeSelf)
            {
                continue;
            }

            Vector3 targetLocalPos = transform.InverseTransformPoint(m_targets[i].position);
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_camera.aspect);
        }

        size += m_screenEdgeBuffer;

        size = Mathf.Max(size, m_minSize);

        return size;
    }

//    public void SetupStartPositionAndSize()
//    {
//        FindAveragePosition();
//        transform.position = m_desiredPosition;
//        m_camera.orthographicSize = FindRequiredSize();
//    }
}