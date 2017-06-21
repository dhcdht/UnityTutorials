using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int m_numRoundsToWin = 5;
    public float m_startDelay = 3.0f;
    public float m_endDelay = 3.0f;
    public CameraControl m_cameraControl;
    public Text m_messageText;
    public GameObject m_tankPrefab;
    public TankManager[] m_tanks;

    private int m_roundNumber;
    private WaitForSeconds m_startWait;
    private WaitForSeconds m_endWait;
    private TankManager m_roundWinner;
    private TankManager m_gameWinner;

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
        for (int i = 0; i < m_tanks.Length; i++)
        {
            m_tanks[i].m_instance =
                Instantiate(m_tankPrefab, m_tanks[i].m_spawnPoint.position, m_tanks[i].m_spawnPoint.rotation) as
                    GameObject;
            m_tanks[i].m_playerNumber = i + 1;
            m_tanks[i].Setup();
        }
    }

    private void SetCameraTargets()
    {
        var targets = new Transform[m_tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = m_tanks[i].m_instance.transform;
        }

        m_cameraControl.m_targets = targets;
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());

        yield return StartCoroutine(RoundPlaying());

        yield return StartCoroutine(RoundEnding());

        if (m_gameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();

        m_cameraControl.ResetStartPositionAndSize();

        m_roundNumber++;
        m_messageText.text = "Round " + m_roundNumber;

        yield return m_startWait;
    }

    private IEnumerator RoundPlaying()
    {
        EnableTankControl();

        m_messageText.text = string.Empty;

        while (!IsOnlyOneTankLeft())
        {
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        DisableTankControl();

        m_roundWinner = null;
        m_roundWinner = GetRoundWinner();

        if (m_roundWinner != null)
        {
            m_roundWinner.m_wins++;
        }

        var message = EndMessage();
        m_messageText.text = message;

        yield return m_endWait;
    }

    private void ResetAllTanks()
    {
        for (int i = 0; i < m_tanks.Length; i++)
        {
            m_tanks[i].Reset();
        }
    }

    private void EnableTankControl()
    {
        for (int i = 0; i < m_tanks.Length; i++)
        {
            m_tanks[i].EnableControl();
        }
    }

    private void DisableTankControl()
    {
        for (int i = 0; i < m_tanks.Length; i++)
        {
            m_tanks[i].DisableControl();
        }
    }

    private bool IsOnlyOneTankLeft()
    {
        var numTanksLeft = 0;
        for (int i = 0; i < m_tanks.Length; i++)
        {
            if (m_tanks[i].m_instance.activeSelf)
            {
                numTanksLeft++;
            }
        }

        return numTanksLeft <= 1;
    }

    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < m_tanks.Length; i++)
        {
            if (m_tanks[i].m_instance.activeSelf)
            {
                return m_tanks[i];
            }
        }

        return null;
    }

    private string EndMessage()
    {
        var message = "Draw!";

        if (m_roundWinner != null)
        {
            message = m_roundWinner.m_coloredPlayerText + " Wins The Round!";
        }
        message += "\n\n\n\n";

        for (int i = 0; i < m_tanks.Length; i++)
        {
            message += m_tanks[i].m_coloredPlayerText + " : " + m_tanks[i].m_wins + " Wins\n";
        }

        if (m_gameWinner != null)
        {
            message = m_gameWinner.m_coloredPlayerText + " Wins The Game!";
        }

        return message;
    }
}