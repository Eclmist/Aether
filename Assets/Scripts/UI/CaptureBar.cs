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

    private float m_redCaptureAmount;

    private float m_blueCaptureAmount;

    private const float m_maximumCaptureAmount = 300.0f;

    // Update is called once per frame
    void Update()
    {
        TowerBase[] towers = GameManager.Instance.GetTowers();

        foreach (TowerBase tower in towers)
        {
            TowerBase.CaptureState captureState = tower.GetCaptureState();
            if (captureState.GetLeadingTeam() == Team.TEAM_ONE)
                m_redCaptureAmount += captureState.GetCapturePercentage();
            else 
                m_blueCaptureAmount += captureState.GetCapturePercentage();
        }

        m_redTeamBar.fillAmount = (m_redCaptureAmount / m_maximumCaptureAmount);
        m_blueTeamBar.fillAmount = (m_blueCaptureAmount / m_maximumCaptureAmount);
    }
}
