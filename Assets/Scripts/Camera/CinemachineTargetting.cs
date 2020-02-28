using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineTargetting : MonoBehaviour
{
    private PlayerManager m_PlayerManager;

    private CinemachineVirtualCameraBase m_VirtualCam;

    private bool m_FirstRun = true;

    void Awake()
    {
        m_PlayerManager = FindObjectOfType<PlayerManager>();
        m_VirtualCam = GetComponent<CinemachineVirtualCameraBase>();
    }

    void Update()
    {
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
}
