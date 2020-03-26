using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisplayOnHover : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Used to display health and crosshair overlay")]
    private GameObject m_CanvasGroup;

    private bool m_IsHovered;

    public void OnHover()
    {
        m_IsHovered = true;
    }

    public void Update()
    {
        m_CanvasGroup?.SetActive(m_IsHovered);
    }

    public void LateUpdate()
    {
        m_IsHovered = false;
    }
}
