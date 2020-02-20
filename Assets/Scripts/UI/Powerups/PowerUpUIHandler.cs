using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpUIHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject m_JumpPowerUpIconPrefab;
    [SerializeField]
    private GameObject m_SpeedPowerUpIconPrefab;

    [SerializeField]
    private const float m_BuffDuration = 5.0f;

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
        {
            activatedIcon.transform.SetParent(this.gameObject.transform, false);
            StartCoroutine(DelayDestruction(activatedIcon));
        }
    }

    private IEnumerator DelayDestruction(GameObject icon) {
        yield return new WaitForSeconds(m_BuffDuration);
        Destroy(icon);
    } 
}
