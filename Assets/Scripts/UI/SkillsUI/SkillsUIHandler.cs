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

    private Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        SwitchIcons();
    }

    void Update()
    {
        // Debug.Log(m_PrimaryIcon.sprite);
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

    public void SwitchIcons()
    {
        StartCoroutine(SwitchUIIcons());
    }

    private IEnumerator SwitchUIIcons()
    {
        if (m_PrimaryIcon != null && m_SecondaryIcon != null)
        {
            m_Animator.SetTrigger("FadeIcons");
            yield return new WaitForSeconds(1.5f);
            SwitchSprites();
            m_Animator.SetTrigger("ReappearIcons");
        }
    }

    private void SwitchSprites()
    {
        Sprite tempIcon = m_PrimaryIcon.sprite;
        m_PrimaryIcon.sprite = m_SecondaryIcon.sprite;
        m_SecondaryIcon.sprite = tempIcon;
    }
}
