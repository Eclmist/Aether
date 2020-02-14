using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerUpTimer : MonoBehaviour
{
    [SerializeField]
    private Image m_Filler;

    [SerializeField]
    private Image m_Icon;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (m_Filler.fillAmount > 0.0f) 
        {
            m_Filler.fillAmount -= (0.20f * Time.deltaTime);
        } 

        if (m_Filler.fillAmount >= 0.5f)
        {
            SetColoredIcon();
        }
        else if (m_Filler.fillAmount > 0.0f) {
            FadeIcon();
        }
        else {
            SetWhiteIcon();
        }
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
        m_Icon.color = new Color(m_Filler.color.r, m_Filler.color.g, m_Filler.color.b, m_Icon.color.a);
    }

    private void SetColoredIcon(float alpha)
    {
        m_Icon.color = new Color(m_Filler.color.r, m_Filler.color.g, m_Filler.color.b, alpha);
    }

    private void FadeIcon() 
    {
        float newAlpha = m_Icon.color.a - (0.40f * Time.deltaTime);

        if (m_Icon.color.r == 255 && m_Icon.color.g == 255 && m_Icon.color.r == 255) 
        {
            SetColoredIcon(newAlpha);
        }
        else {
            SetWhiteIcon(newAlpha);
        }

        Debug.Log(m_Icon.color.r + " : " + m_Icon.color.a);
    }
}
