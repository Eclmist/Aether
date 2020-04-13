using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProgress : MonoBehaviour
{
    public int m_PlayerIndex = 0;
    public Image m_SliderImage;


    // Update is called once per frame
    void Update()
    {
        if (m_SliderImage != null)
        {
            m_SliderImage.fillAmount = GameManager.Instance.GetPlayerProgress(m_PlayerIndex);
        }
    }
}
