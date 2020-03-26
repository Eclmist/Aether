using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * DELETE THIS AFTER E4! It is a shitty class
 */

[RequireComponent(typeof(Image))]
public class UIHealthSlider : MonoBehaviour
{
    [SerializeField]
    private HealthHandler m_HealthHandler;

    public Text m_HealthText;

    private Image m_Image;

    // Start is called before the first frame update
    void Start()
    {
        m_Image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_HealthHandler)
            m_Image.fillAmount = m_HealthHandler.GetPercentageHealth();

        if (m_HealthText)
            m_HealthText.text = (int)m_HealthHandler.GetHealth() + " / " + (int)m_HealthHandler.m_MaxHealth;
    }
}
