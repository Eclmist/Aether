using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerSceneController : Singleton<MultiplayerSceneController>
{
    public Animator m_NotificationAnimator;
    public Text m_NotificationText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("NotifyCTFGameMode");
    }

    IEnumerator NotifyCTFGameMode()
    {
        yield return new WaitForSeconds(2);
        UINotify("King Of The Hill");
    }

    public void UINotify(string message)
    {
        if (m_NotificationText != null)
            m_NotificationText.text = message;
        if (m_NotificationAnimator != null)
            m_NotificationAnimator.SetTrigger("Open");
    }
}
