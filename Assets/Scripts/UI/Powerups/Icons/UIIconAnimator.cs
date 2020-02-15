using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIconAnimator : MonoBehaviour
{
    [SerializeField]
    private Image m_icon;

    [SerializeField]
    private Image m_fadedIcon;

    [SerializeField]
    private Image m_fillArea;

    private Image m_box;

    private Outline m_outline;

    private UIIconTimer m_iconTimer;

    private const float m_ImageDecrementFactor = 0.20f;

    private const float m_FillAreaDecrementFactor = 0.04f;

    // Start is called before the first frame update
    void Start()
    {
        m_iconTimer = GetComponent<UIIconTimer>();
        m_box = GetComponent<Image>();
        m_outline = GetComponent<Outline>();

        ActivateIcon();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_iconTimer == null)
        {
            return;
        }

        if (m_iconTimer.HasCountdownFinishedHalfway())
        {
            DecreaseAlphaValues();    
        }

        if (!m_iconTimer.HasCountdownFinished()) 
        {
            FillIcon(m_iconTimer.GetPercentageLeft());
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
        DecreaseAlphaValue(m_icon, m_ImageDecrementFactor * 2);
        DecreaseAlphaValue(m_fadedIcon, m_ImageDecrementFactor);
        DecreaseAlphaValue(m_box, m_ImageDecrementFactor * 2);
        DecreaseAlphaValue(m_outline, m_ImageDecrementFactor);
        DecreaseAlphaValue(m_fillArea, m_FillAreaDecrementFactor);
    }

    public void ActivateIcon()
    {
        if (m_iconTimer != null)
        {
            m_iconTimer.StartTimer();
        }
    }

    public void FillIcon(float amount)
    {
        if (m_icon != null)
        {
            m_icon.fillAmount = amount;
        }
    }
}
