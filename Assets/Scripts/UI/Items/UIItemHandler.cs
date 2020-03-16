using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemHandler : MonoBehaviour
{
    [SerializeField]
    private Image m_Icon;

    [SerializeField] 
    private int m_UsageInstances;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool CanSignal()
    {
        return m_Icon.color.a >= 0.0f;
    }

    private void SetColoredIcon(float alpha)
    {
        m_Icon.color = new Color(m_Icon.color.r, m_Icon.color.g, m_Icon.color.b, alpha);
    }

    public void ReduceInstances()
    {
        if (CanSignal())
        {
            SetColoredIcon(m_Icon.color.a - (1.0f / m_UsageInstances));
        }
    }
    
}
