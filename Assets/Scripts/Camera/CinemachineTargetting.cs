using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineTargetting : MonoBehaviour
{
    private PlayerManager m_PlayerManager;

    private CinemachineVirtualCameraBase m_VirtualCam;

    private bool m_FirstRun = true;

    private void Awake()
    {
        m_PlayerManager = FindObjectOfType<PlayerManager>();
        m_VirtualCam = GetComponent<CinemachineVirtualCameraBase>();
    }

    private void Update()
    {
        // Switch this to event based. Should subscribe to either game start... or when players loaded
        if (m_FirstRun)
        {
            if (m_PlayerManager.GetLocalPlayer() == null)
                return;

            Transform playerTransform = m_PlayerManager.GetLocalPlayer().transform;
            m_VirtualCam.Follow = playerTransform;
            m_VirtualCam.LookAt = playerTransform;
            m_FirstRun = false;
        }
    }

    private void OnPlayerDeath()
    {
        List<Player> players = PlayerManager.Instance.GetTeamMembers();

        if (players.Count == 0)
            return;

        m_VirtualCam.Follow = players[0].transform;
        m_VirtualCam.LookAt = players[0].transform;
    }
}
