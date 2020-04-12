using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Collections;

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

        localPlayer.PlayerDrop += OnPlayerDrop;
        localPlayer.PlayerRespawn += OnPlayerRespawn;
    }

    private void OnPlayerDrop()
    {
        Debug.Log("Drop");
        m_VirtualCam.Follow = null;
        m_VirtualCam.LookAt = null;
        m_VirtualCam.transform.Translate(new Vector3(0, 25.0f - m_VirtualCam.transform.position.y, 0));
    }

    private void OnPlayerRespawn()
    {
        Debug.Log("Respawn");
        Player player = PlayerManager.Instance.GetLocalPlayer();
        m_VirtualCam.Follow = player.transform;
        m_VirtualCam.LookAt = player.transform;
    }
}
