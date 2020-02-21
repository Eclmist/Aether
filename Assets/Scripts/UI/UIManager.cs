using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private GameObject m_powerUpHandler;
    public void ActivatePowerupIcon(UIPowerUpSignals signal)
    {
        if (m_powerUpHandler != null)
        {
            PowerUpUIHandler handler = m_powerUpHandler.GetComponent<PowerUpUIHandler>();
            if (handler != null && signal != null) 
            {
                handler.ActivateIcon(signal);
            }
        }
    }

}
