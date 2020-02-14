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

    private float m_CountdownTimer;

    private Color m_IconColor;

    private float m_alphaDecrement;

    // Start is called before the first frame update
    void Start()
    {
        m_IconColor = new Color(m_Icon.color.r, m_Icon.color.g, m_Icon.color.b, m_Icon.color.a);
        m_alphaDecrement = 0.40f * Time.deltaTime;
        SetWhiteIcon(0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CountdownTimer > 0.0f)
        {
            UpdateTimer();
            FadeIcon();
        }
    }

    public void Activate()
    {
        Debug.Log("Here Again");
        m_CountdownTimer = m_Timer;
        SetColoredIcon(1.0f);
    }

    private void SetWhiteIcon(float alpha) 
    {
        m_Icon.color = new Color(255, 255, 255, alpha);
    }

    private void SetColoredIcon(float alpha)
    {
        m_Icon.color = new Color(m_IconColor.r, m_IconColor.g, m_IconColor.b, alpha);
    }

    private void UpdateTimer()
    {
        m_CountdownTimer -= Time.deltaTime;
    }

    private void FadeIcon() 
    {
        if (m_CountdownTimer <= 0.0f || m_CountdownTimer >= 2.5f) 
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
