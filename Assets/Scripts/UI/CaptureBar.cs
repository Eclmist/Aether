using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureBar : MonoBehaviour
{
    [SerializeField]
    private Image m_redTeamBar;

    [SerializeField]
    private Image m_blueTeamBar;

    private CanvasGroup m_CanvasGroup;

    private float m_redCaptureAmount;

    private float m_blueCaptureAmount;

    private const float m_maximumCaptureAmount = 300.0f;

    void Start()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        m_redCaptureAmount = 0.0f;
        m_blueCaptureAmount = 0.0f;

        TowerBase[] towers = null;

        /*foreach (TowerBase tower in towers)
        {
            TowerBase.CaptureState captureState = tower.GetCaptureState();
            if (captureState.GetLeadingTeam() == Team.TEAM_ONE)
                m_redCaptureAmount += captureState.GetCapturePercentage();
            else 
                m_blueCaptureAmount += captureState.GetCapturePercentage();
        }*/

        m_redTeamBar.fillAmount = (m_redCaptureAmount / m_maximumCaptureAmount);
        m_blueTeamBar.fillAmount = (m_blueCaptureAmount / m_maximumCaptureAmount);

        foreach (TowerBase tower in towers)
        {
            //if (tower.GetBeingCaptured())
            //{
            //    ActivateBar();
            //    return;
            //}
        }

        DeactivateBar();
    }

    public void ActivateBar()
    {
        if (m_CanvasGroup != null)
            m_CanvasGroup.alpha = 1.0f;
    }

    public void DeactivateBar()
    {
        if (m_CanvasGroup != null)
            m_CanvasGroup.alpha = 0.2f;
    }
}
