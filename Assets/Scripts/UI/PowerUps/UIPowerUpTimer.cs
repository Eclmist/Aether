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

    private Color m_WhiteColor = new Color(255, 255, 255, 100);

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Filler.fillAmount > 0.0f) 
        {
            m_Filler.fillAmount -= (0.20f * Time.deltaTime);
        } 

        if (m_Filler.fillAmount >= 0.5f)
        {
            m_Icon.color = m_Filler.color;
        }
        else if (m_Filler.fillAmount > 0.0f) {
            FlipIconColors();
        }
        else {
            m_Icon.color = m_WhiteColor;
        }
    }

    private void FlipIconColors() 
    {
        if (m_Icon.color == m_WhiteColor) 
        {
            m_Icon.color = m_Filler.color;
        }
        else {
            m_Icon.color = m_WhiteColor;
        }
    }
}
