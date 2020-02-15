using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPanelHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject m_JumpPowerUpIconPrefab;
    [SerializeField]
    private GameObject m_SpeedPowerUpIconPrefab;

    // Simple call to Activate Jump Icon on Power Up Panel. 
    public void ActivateJumpIcon()
    {
        if (m_JumpPowerUpIconPrefab != null)
        {
            GameObject jumpIcon = Instantiate(m_JumpPowerUpIconPrefab) as GameObject;
            jumpIcon.transform.SetParent(this.gameObject.transform, false);
        }
    }

    // Simple call to Activate Jump Icon on Power Up Panel. 
    public void ActivateSpeedIcon()
    {
        if (m_SpeedPowerUpIconPrefab != null)
        {
            GameObject speedIcon = Instantiate(m_SpeedPowerUpIconPrefab) as GameObject;
            speedIcon.transform.SetParent(this.gameObject.transform, false);
        }
    }
}
