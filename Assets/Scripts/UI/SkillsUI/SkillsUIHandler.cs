using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsUIHandler : MonoBehaviour
{

    private Animator m_Animator;
    private bool isPrimary;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        isPrimary = true;
    }

    public void SwitchSkills()
    {
        SwitchSkillCoroutine();
    }

    private void SwitchSkillCoroutine()
    {
        if (isPrimary)
        {
            m_Animator.SetTrigger("PrimaryToSecondary");
        } else
        {
            
        }
        
    }
}
