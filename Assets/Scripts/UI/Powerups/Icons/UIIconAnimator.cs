using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIconAnimator : MonoBehaviour
{
    [SerializeField]
    private Image m_Icon;

    [SerializeField]
    private Image m_FadedIcon;

    [SerializeField]
    private Image m_FillArea;

    private Image m_Box;

    private Outline m_Outline;

    private UIIconTimer m_IconTimer;

    private const float m_ImageDecrementFactor = 0.20f;

    private const float m_FillAreaDecrementFactor = 0.04f;

    // Start is called before the first frame update
    void Start()
    {
        m_IconTimer = GetComponent<UIIconTimer>();
        m_Box = GetComponent<Image>();
        m_Outline = GetComponent<Outline>();

        ActivateIcon();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IconTimer == null)
        {
            return;
        }

        if (m_IconTimer.HasCountdownFinishedHalfway())
        {
            DecreaseAlphaValues();    
        }

        if (!m_IconTimer.HasCountdownFinished()) 
        {
            FillIcon(m_IconTimer.GetPercentageLeft());
        }
        else {
            Destroy(this.gameObject);
        }
    }

    private void DecreaseAlphaValue(Image image, float decrementFactor)
    {
        if (image != null)
        {
            var imageColor = image.color;
            imageColor.a -= decrementFactor * Time.deltaTime;
            image.color = imageColor;
        }
    }

    private void DecreaseAlphaValue(Outline outline, float decrementFactor)
    {
        if (outline != null)
        {
            var outlineColor = outline.effectColor;
            outlineColor.a -= decrementFactor * Time.deltaTime;
            outline.effectColor = outlineColor;
        }
    }

    private void DecreaseAlphaValues()
    {
        DecreaseAlphaValue(m_Icon, m_ImageDecrementFactor * 2);
        DecreaseAlphaValue(m_FadedIcon, m_ImageDecrementFactor);
        DecreaseAlphaValue(m_Box, m_ImageDecrementFactor * 2);
        DecreaseAlphaValue(m_Outline, m_ImageDecrementFactor);
        DecreaseAlphaValue(m_FillArea, m_FillAreaDecrementFactor);
    }

    public void ActivateIcon()
    {
        if (m_IconTimer != null)
        {
            m_IconTimer.StartTimer();
        }
    }

    public void FillIcon(float amount)
    {
        if (m_Icon != null)
        {
            m_Icon.fillAmount = amount;
        }
    }
}
