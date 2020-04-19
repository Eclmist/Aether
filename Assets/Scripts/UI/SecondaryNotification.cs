using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondaryNotification : MonoBehaviour
{
    public Text m_UIText;

    public void Start()
    {
        Destroy(gameObject, 8);
    }

    public void SetUIText(string text)
    {
        m_UIText.text = text;
    }
}
