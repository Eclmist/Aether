using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private CaptureState m_CaptureState;

    public CaptureState GetCaptureState()
    {
        return m_CaptureState;
    }

    public struct CaptureState
    {
        private int m_CapturePercentage;
        private int m_TeamId;

        public bool IsCaptured()
        {
            return m_CapturePercentage == 100;
        }

        public int GetCapturePercentage()
        {
            return m_CapturePercentage;
        }

        public int GetTeam()
        {
            return m_TeamId;
        }
    }
}
