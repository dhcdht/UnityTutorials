using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int m_numRoundsToWin = 5;
    public float m_startDelay = 3.0f;
    public float m_endDelay = 3.0f;
    public CameraControl m_cameraControl;
    public TankManager[] m_tanks;
    

    private WaitForSeconds m_startWait;
    private WaitForSeconds m_endWait;


    // Use this for initialization
    private void Start()
    {
        m_startWait = new WaitForSeconds(m_startDelay);
        m_endWait = new WaitForSeconds(m_endDelay);

        SpawnAllTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }

    private void SpawnAllTanks()
    {
        for (int i = 0; i < m_tan; i++)
        {
            
        }
    }

    private void SetCameraTargets()
    {
    }

    private IEnumerator GameLoop()
    {
    }
}