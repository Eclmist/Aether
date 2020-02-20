using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpUIHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject m_JumpPowerUpIconPrefab;
    [SerializeField]
    private GameObject m_SpeedPowerUpIconPrefab;

    public void ActivateIcon(UIPowerUpSignals signal) 
    {
        GameObject activatedIcon = null; 

        switch (signal)
        {
        case UIPowerUpSignals.JUMP_SIGNAL:
            if (m_JumpPowerUpIconPrefab != null)
                activatedIcon = Instantiate(m_JumpPowerUpIconPrefab) as GameObject;
            break;
        case UIPowerUpSignals.SPEED_SIGNAL:
            if (m_SpeedPowerUpIconPrefab != null)
                activatedIcon = Instantiate(m_SpeedPowerUpIconPrefab) as GameObject;
            break;
        default: 
            return;
        }

        if (activatedIcon != null)
            activatedIcon.transform.SetParent(this.gameObject.transform, false);
    }
}
