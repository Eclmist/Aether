using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIconTimer : MonoBehaviour
{
    [SerializeField]
    private float m_setDuration;

    private float m_actualDuration;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        m_actualDuration -= Time.deltaTime;
    }

    public bool HasCountdownFinished()
    {
        return m_actualDuration <= 0.0f;
    }

    public bool HasCountdownFinishedHalfway()
    {
        return GetPercentageLeft() <= 0.50f;
    }

    // Gets a value between 0 and 1. 
    public float GetPercentageLeft()
    {
        return m_actualDuration / m_setDuration;
    }

    public void StartTimer()
    {
        m_actualDuration = m_setDuration;
    }
}
