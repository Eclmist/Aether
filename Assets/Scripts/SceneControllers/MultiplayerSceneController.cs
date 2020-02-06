using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSceneController : MonoBehaviour
{
    public Animator m_NotificationAnimator;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("NotifyCTFGameMode");
    }

    IEnumerator NotifyCTFGameMode()
    {
        yield return new WaitForSeconds(2);
        if (m_NotificationAnimator != null)
            m_NotificationAnimator.SetTrigger("Open");
    }
}
