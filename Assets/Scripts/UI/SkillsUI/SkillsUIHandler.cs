using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsUIHandler : MonoBehaviour
{
    [SerializeField]
    private Image m_PrimaryIcon;

    [SerializeField]
    private Image m_SecondaryIcon;

    [SerializeField]
    private Image m_TertiaryIcon;

    private Sprite m_NullSprite;

    private int m_SkillsIndex;

    // Start is called before the first frame update
    void Start()
    {
        m_NullSprite = m_PrimaryIcon.sprite;
        m_SkillsIndex = 0;
    }

    public void HandleSkillPickUp(ItemSkill itemSkill)
    {
        if (itemSkill == null)
            return;

        Image icon = itemSkill.GetSkillsIcon();
        if (m_PrimaryIcon.sprite.name == m_NullSprite.name) 
            SavePrimaryIcon(icon);
        else if (m_SecondaryIcon.sprite.name == m_NullSprite.name)
            SaveSecondaryIcon(icon);
        else if (m_TertiaryIcon.sprite.name == m_NullSprite.name)
            SaveTertiaryIcon(icon);
    }

    // As of now, just simply remove the skill icon but plan to add cycle system in the future for player convenience.
    public void RemoveUsedSkill()
    {
        m_PrimaryIcon.sprite = m_SecondaryIcon.sprite;
        m_SecondaryIcon.sprite = m_TertiaryIcon.sprite; 
        m_TertiaryIcon.sprite = m_NullSprite;
    }
    public void SavePrimaryIcon(Image image)
    {
        if (image != null)
            m_PrimaryIcon.sprite = image.sprite;
    }
    public void SaveSecondaryIcon(Image image)
    {
        if (image != null)
            m_SecondaryIcon.sprite = image.sprite;
    }

    public void SaveTertiaryIcon(Image image)
    {
        if (image != null)
            m_TertiaryIcon.sprite = image.sprite;
    }

    public void SwitchSkillsSprites()
    {
        SwitchSpriteIcons();
        IncrementIndex();
    }

    private void SwitchSpriteIcons()
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
        m_SkillsIndex = (m_SkillsIndex + 1) % 3;
    }

    public int GetSkillsIndex()
    {
        return m_SkillsIndex;
    }
}
