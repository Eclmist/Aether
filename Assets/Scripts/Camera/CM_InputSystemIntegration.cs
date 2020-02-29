using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CM_InputSystemIntegration : MonoBehaviour
{
    void Awake()
    {
    }

    void Update()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }

    public float GetAxisCustom(string axisName)
    {
        Vector2 lookDelta = AetherInput.GetPlayerActions().Look.ReadValue<Vector2>();
        return axisName == "Mouse X" ? lookDelta.x : lookDelta.y;
    }
}
