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
}