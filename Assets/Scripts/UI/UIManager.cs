using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private GameObject m_powerUpHandler;

    public static string JumpSignal = "Jump";
    public static string SpeedSignal = "Speed";

    public void ActivatePowerupIcon(string signal)
    {
        if (m_powerUpHandler != null)
        {
            PowerUpPanelHandler handler = m_powerUpHandler.GetComponent<PowerUpPanelHandler>();
            if (handler != null) 
            {
                switch (signal)
                {
                case "Jump":
                    handler.ActivateJumpIcon();
                    break;
                case "Speed":
                    handler.ActivateSpeedIcon();
                    break;
                default: 
                    return;
                }
            }
        }
    }

}
