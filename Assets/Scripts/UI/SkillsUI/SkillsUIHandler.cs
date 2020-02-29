using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsUIHandler : MonoBehaviour
{

    private Animator m_Animator;

    [SerializeField]
    private Image m_PrimaryIcon;

    [SerializeField]
    private Image m_SecondaryIcon;

    [SerializeField]
    private Image m_TertiaryIcon;

    private int m_SkillsIndex;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_SkillsIndex = 0;
    }

    public void SavePrimaryIcon(Image image)
    {
        if (image != null)
        {
            m_PrimaryIcon = image;
        }
    }

    public void SaveSecondaryIcon(Image image)
    {
        if (image != null)
        {
            m_SecondaryIcon = image;
        }
    }

    public void SaveTertiaryIcon(Image image)
    {
        if (image != null)
        {
            m_TertiaryIcon = image;
        }
    }

    public void SwitchSkills()
    {
        IncrementIndex();
        SwitchSkillCoroutine();
        SwitchSpritesCoroutine();
    }

    private void SwitchSkillCoroutine()
    {
        if (m_SkillsIndex == 0)
        {
            return;
        }
        
        if (m_SkillsIndex == 1)
        {
            return;
        }

        if (m_SkillsIndex == 2)
        {
            return;
        }
    }

    private void SwitchSpritesCoroutine()
    {
        if (m_SkillsIndex == 0)
        {
            Sprite temp = m_PrimaryIcon.sprite;
            Sprite temp2 = m_TertiaryIcon.sprite;
            m_PrimaryIcon.sprite = m_SecondaryIcon.sprite;            
            m_TertiaryIcon.sprite = temp;
            m_SecondaryIcon.sprite = temp2;
            return;
        }

        if (m_SkillsIndex == 1)
        {
            Sprite temp = m_SecondaryIcon.sprite;
            Sprite temp2 = m_PrimaryIcon.sprite;
            m_SecondaryIcon.sprite = m_TertiaryIcon.sprite;
            m_PrimaryIcon.sprite = temp;
            m_TertiaryIcon.sprite = temp2;
            return;
        }

        if (m_SkillsIndex == 2)
        {
            Sprite temp = m_TertiaryIcon.sprite;
            Sprite temp2 = m_SecondaryIcon.sprite;
            m_TertiaryIcon.sprite = m_PrimaryIcon.sprite;         
            m_SecondaryIcon.sprite = temp;
            m_PrimaryIcon.sprite = temp2;
            return;
        }
    }

    public void IncrementIndex()
    {
        if (m_SkillsIndex == 2)
        {
            m_SkillsIndex = 0;
        } else
        {
            m_SkillsIndex++;
        }
    }
}
