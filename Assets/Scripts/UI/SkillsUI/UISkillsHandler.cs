using UnityEngine;
using UnityEngine.UI;

public class UISkillsHandler : MonoBehaviour
{
    [SerializeField]
    private Image m_PrimaryIcon;

    [SerializeField]
    private Image m_SecondaryIcon;

    [SerializeField]
    private Image m_TertiaryIcon;

    private int m_IconCount;

    // Image.sprite = sprite 

    public void Start()
    {
        ChangeAlphaValue(m_PrimaryIcon, 0.0f);
        ChangeAlphaValue(m_SecondaryIcon, 0.0f);
        ChangeAlphaValue(m_TertiaryIcon, 0.0f);
        m_IconCount = 0;
    }

    private void ChangeAlphaValue(Image image, float value)
    {
        if (image == null)
            return;

        var tempColor = image.color;
        tempColor.a = value;
        image.color = tempColor;
    }

    public void HandleSkillPickUp(SkillItem itemSkill)
    {
        if (itemSkill == null)
            return;

        Image icon = itemSkill.GetSkillsIcon();

        if (icon == null)
            return;

        AssignIcons(icon);

        m_IconCount ++;
    }

    public void RemoveUsedSkill()
    {
        if (m_IconCount == 0 || m_PrimaryIcon == null 
            || m_SecondaryIcon == null || m_TertiaryIcon == null)
            return;

        if (m_IconCount == 3)
        {
            SwitchSpriteIcons();
            ChangeAlphaValue(m_TertiaryIcon, 0.0f);
        }

        if (m_IconCount == 2)
        {
            SwitchSpriteIcons();
            ChangeAlphaValue(m_SecondaryIcon, 0.0f);
        }

        if (m_IconCount == 1)
            ChangeAlphaValue(m_PrimaryIcon, 0.0f);    

        m_IconCount--;
    }

    public void SwitchSpriteIcons()
    {
        if (m_IconCount == 3)
            SwitchThreeIcons();

        if (m_IconCount == 2)
            SwitchTwoIcons();
    }

    private void SwitchThreeIcons()
    {
        if(m_PrimaryIcon == null || m_SecondaryIcon == null ||  m_TertiaryIcon == null)
            return;

        Sprite tempSprite = m_PrimaryIcon.sprite;
        m_PrimaryIcon.sprite = m_SecondaryIcon.sprite;
        m_SecondaryIcon.sprite = m_TertiaryIcon.sprite;
        m_TertiaryIcon.sprite = tempSprite;
    }

    private void SwitchTwoIcons()
    {
        Sprite tempSprite = m_PrimaryIcon.sprite;
        m_PrimaryIcon.sprite = m_SecondaryIcon.sprite;
        m_SecondaryIcon.sprite = tempSprite;
    }

    private void AssignIcons(Image icon)
    {
        if (m_IconCount == 3)
            return;

        if (m_IconCount == 0)
        {
            m_PrimaryIcon.sprite = icon.sprite;
            ChangeAlphaValue(m_PrimaryIcon, 1.0f);
        }

        if (m_IconCount == 1)
        {
            m_SecondaryIcon.sprite = icon.sprite;
            ChangeAlphaValue(m_SecondaryIcon, 1.0f);
        }

        if (m_IconCount == 2)
        {
            m_TertiaryIcon.sprite = icon.sprite;
            ChangeAlphaValue(m_TertiaryIcon, 1.0f);
        }
    }


}
