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

    private ItemSkill m_PrimarySkill;

    private ItemSkill m_SecondarySkill;

    private ItemSkill m_TertiarySkill;

    private int m_SkillsIndex;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_SkillsIndex = 0;
    }

    public void HandleSKillPickUp(ItemSkill itemSkill)
    {
        if (itemSkill == null)
            return; 
        
        Image icon = itemSkill.GetComponent<Image>();

        if (m_PrimarySkill == null)
        {
            SavePrimarySkill(itemSkill, icon);
        } 
        else if (m_SecondarySkill == null)
        {
            SaveSecondarySkill(itemSkill, icon);
        }
        else if (m_TertiarySkill == null) 
        {
            SaveTertiarySkill(itemSkill, icon);
        }
    }

    public void RemoveUsedSkill()
    {
        if (m_PrimarySkill == null)
            return;

        m_PrimarySkill = m_SecondarySkill;
        m_PrimaryIcon = m_SecondaryIcon;

        m_SecondarySkill = m_TertiarySkill;
        m_SecondaryIcon = m_TertiaryIcon;

        m_TertiarySkill = null;
        m_TertiaryIcon = null;
    }

    public void SaveSecondarySkill(ItemSkill itemskill, Image image)
    {
        if (image != null)
        {
            m_SecondarySkill = itemskill;
            m_SecondaryIcon = image;
        }
    }

    public void SaveTertiarySkill(ItemSkill itemskill, Image image)
    {
        if (image != null)
        {
            m_TertiarySkill = itemskill;
            m_TertiaryIcon = image;
        }
    }

    public void SavePrimarySkill(ItemSkill itemskill, Image image)
    {
        if (image != null)
        {
            m_PrimarySkill = itemskill;
            m_PrimaryIcon = image;
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
