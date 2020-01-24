using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineTargetting : MonoBehaviour
{
    private CinemachineVirtualCameraBase m_VirtualCam;

    void Start()
    {
        m_VirtualCam = GetComponent<CinemachineVirtualCameraBase>();
    }

    void Update()
    {
        if (m_VirtualCam.Follow == null && m_VirtualCam.LookAt == null)
        {
            Transform playerTransform = PlayerManager.GetLocalPlayerInstance().transform;
            m_VirtualCam.Follow = playerTransform;
            m_VirtualCam.LookAt = playerTransform;
        }
    }
}
