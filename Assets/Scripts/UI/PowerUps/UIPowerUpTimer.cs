using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerUpTimer : MonoBehaviour
{

    [SerializeField]
    private Image m_Icon;

    [SerializeField]
    private float m_Timer;

    private Color m_IconColor;

    private float m_alphaDecrement;

    // Start is called before the first frame update
    void Start()
    {
        m_IconColor = new Color(m_Icon.color.r, m_Icon.color.g, m_Icon.color.b, m_Icon.color.a);
        m_alphaDecrement = 0.40f * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
        FadeIcon();
    }

    private void SetWhiteIcon() 
    {
        m_Icon.color = new Color(255, 255, 255, m_Icon.color.a);
    }

    private void SetWhiteIcon(float alpha) 
    {
        m_Icon.color = new Color(255, 255, 255, alpha);
    }

    private void SetColoredIcon()
    {
        m_Icon.color = new Color(m_IconColor.r, m_IconColor.g, m_IconColor.b, m_Icon.color.a);
    }

    private void SetColoredIcon(float alpha)
    {
        m_Icon.color = new Color(m_IconColor.r, m_IconColor.g, m_IconColor.b, alpha);
    }

    private void UpdateTimer()
    {
        m_Timer -= Time.deltaTime;
    }

    private void FadeIcon() 
    {
        if (m_Timer <= 0.0f || m_Timer >= 2.5f) 
        {
            return;
        }

        if (m_Icon.color.r == 255 && m_Icon.color.g == 255 && m_Icon.color.r == 255) 
        {
            SetColoredIcon(m_Icon.color.a - m_alphaDecrement);
        }
        else {
            SetWhiteIcon(m_Icon.color.a - m_alphaDecrement);
        }
    }
}
