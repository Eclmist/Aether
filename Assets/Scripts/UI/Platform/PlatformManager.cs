using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlatformManager : Singleton<PlatformManager>
{
    public event System.Action<PlatformType> PlatformChanged;
    private PlatformType m_platformType;

    [SerializeField]
    private PlatformIconChanger[] m_iconChangers;
    // Start is called before the first frame update
    void Start()
    {
        m_platformType = PlatformType.PS4_CONTROLLER;

        AetherInput.GetUIActions().KeyboardAction.performed += RetrievePlatformTypeCallback;
        AetherInput.GetUIActions().Cancel.performed += RetrievePlatformTypeCallback;
        AetherInput.GetUIActions().Navigate.performed += RetrievePlatformTypeCallback;
        AetherInput.GetUIActions().Click.performed += RetrievePlatformTypeCallback;
        AetherInput.GetUIActions().MiddleClick.performed += RetrievePlatformTypeCallback;
        AetherInput.GetUIActions().RightClick.performed += RetrievePlatformTypeCallback;
    }

    public PlatformType GetPlatformType()
    {
        return m_platformType;
    }

    private void RetrievePlatformTypeCallback(InputAction.CallbackContext ctx)
    {
        string inputButtonType = ctx.control.device.name;

        PlatformType previousPlatformType = m_platformType;

        switch(inputButtonType)
        {
            case "Keyboard":
                m_platformType = PlatformType.KEYBOARD_MOUSE;
                break;
            case "Mouse":
                m_platformType = PlatformType.KEYBOARD_MOUSE;
                break;
            default: 
                m_platformType = PlatformType.PS4_CONTROLLER;
                break;
        }

        if (m_platformType != previousPlatformType)
        {
            PlatformChanged?.Invoke(m_platformType);
        }

    }
}
