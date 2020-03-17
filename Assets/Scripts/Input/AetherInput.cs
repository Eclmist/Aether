using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AetherInput : MonoBehaviour
{
    public static AetherInput Instance;

    private AetherControlSystem m_ControlSystem;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
        m_ControlSystem = new AetherControlSystem();
    }

    private void OnEnable()
    {
        m_ControlSystem.Enable();
    }

    private void OnDisable()
    {
        m_ControlSystem.Disable();
    }

    public static AetherControlSystem.PlayerActions GetPlayerActions()
    {
        return Instance.m_ControlSystem.Player;
    }
    public static AetherControlSystem.UIActions GetUIActions()
    {
        return Instance.m_ControlSystem.UI;
    }

}
