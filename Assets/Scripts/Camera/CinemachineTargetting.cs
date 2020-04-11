using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineTargetting : MonoBehaviour
{
    private CinemachineVirtualCameraBase m_VirtualCam;

    private void Awake()
    {
        m_VirtualCam = GetComponent<CinemachineVirtualCameraBase>();

        PlayerManager.Instance.PlayerListPopulated += TargetLocalPlayer;
    }

    private void TargetLocalPlayer()
    {
        PlayerManager.Instance.PlayerListPopulated -= TargetLocalPlayer;
        Player localPlayer = PlayerManager.Instance.GetLocalPlayer();

        Transform playerTransform = localPlayer.transform;
        m_VirtualCam.Follow = playerTransform;
        m_VirtualCam.LookAt = playerTransform;

        localPlayer.PlayerDead += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        List<Player> players = PlayerManager.Instance.GetTeamMembers();

        if (players.Count == 0)
            return;

        m_VirtualCam.Follow = players[0].transform;
        m_VirtualCam.LookAt = players[0].transform;
    }

    private void OnDestroy()
    {
        if (PlayerManager.HasInstance)
        {
            Player localPlayer = PlayerManager.Instance.GetLocalPlayer();
            if (localPlayer != null)
                localPlayer.PlayerDead -= OnPlayerDeath;
        }
    }
}
