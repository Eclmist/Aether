using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisplayOnHover : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Used to display health and crosshair overlay")]
    private GameObject m_CanvasGroup;

    private float m_HoveredTime = float.MinValue;
    private float m_ActiveTime = 5.0f;

    public void OnHover()
    {
        m_HoveredTime = Time.time;
    }

    public void Update()
    {
        m_CanvasGroup?.SetActive(Time.time - m_HoveredTime < m_ActiveTime);
    }
}
