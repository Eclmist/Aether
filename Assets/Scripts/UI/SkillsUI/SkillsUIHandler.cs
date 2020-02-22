using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsUIHandler : MonoBehaviour
{

    private Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        SwitchSkills();
    }

    void Update()
    {
        // Debug.Log(m_PrimaryIcon.sprite);
    }

    public void SwitchSkills()
    {
        SwitchSkillCoroutine();
    }

    private void SwitchSkillCoroutine()
    {
        m_Animator.SetTrigger("PrimaryToSecondary");
    }
}
