using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineTargetting : MonoBehaviour
{
    private CinemachineVirtualCameraBase m_VirtualCam;

    private bool m_FirstRun = true;

    void Awake()
    {
        m_VirtualCam = GetComponent<CinemachineVirtualCameraBase>();
    }

    void Update()
    {
        if (m_FirstRun)
        {
            Transform playerTransform = PlayerManager.GetLocalPlayerInstance().transform;
            m_VirtualCam.Follow = playerTransform;
            m_VirtualCam.LookAt = playerTransform;
            m_FirstRun = false;
        }
    }
}
