using System;
using UnityEngine;

/// <summary>
/// 这个类管理一个坦克实例
/// </summary>
[Serializable]
public class TankManager
{
    // 
    public Color m_playerColor;
    public Transform m_spawnPoint;
    public int m_playerNumber;
    public string m_coloredPlayerText;
    public GameObject m_instance;
    public int m_wins;

    private TankMovement m_movement;
    private TankShooting m_shooting;
    private GameObject m_canvasGameObject;

    public void Setup()
    {
        m_movement = m_instance.GetComponent<TankMovement>();
        m_shooting = m_instance.GetComponent<TankShooting>();
        m_canvasGameObject = m_instance.GetComponentInChildren<Canvas>().gameObject;

        m_movement.m_playerNumber = m_playerNumber;
        m_shooting.m_playerNumber = m_playerNumber;

        m_coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_playerColor) + ">Player " + m_playerNumber +
                              "</color>";

        var renderers = m_instance.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_playerColor;
        }
    }

    public void DisableControl()
    {
        m_movement.enabled = false;
        m_shooting.enabled = false;

        m_canvasGameObject.SetActive(false);
    }

    public void EnableControl()
    {
        m_movement.enabled = true;
        m_shooting.enabled = true;

        m_canvasGameObject.SetActive(true);
    }

    public void Reset()
    {
        m_instance.transform.position = m_spawnPoint.position;
        m_instance.transform.rotation = m_spawnPoint.rotation;

        m_instance.SetActive(false);
        m_instance.SetActive(true);
    }
}