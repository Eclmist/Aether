using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacterCustomizeUI : MonoBehaviour
{
    [System.Serializable]
    public class ExpansionGroup
    {
        [SerializeField]
        public GameObject m_ExpansionMenu;

        [SerializeField]
        public GameObject m_TriggerButton;
    }

    [SerializeField]
    private ExpansionGroup[] m_ExpansionGroups;

    private Animator[] m_Animators;
    private NavigationGroup[] m_NavGroups;

    private int m_CurrentSelectedIndex = 0;

    private RectTransform m_RectTransform;

    // Start is called before the first frame update
    protected void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_Animators = new Animator[m_ExpansionGroups.Length];
        m_NavGroups = new NavigationGroup[m_ExpansionGroups.Length];

        for (int i = 0; i < m_ExpansionGroups.Length; ++i)
        {
            m_Animators[i] = m_ExpansionGroups[i].m_ExpansionMenu.GetComponent<Animator>();
            m_NavGroups[i] = m_ExpansionGroups[i].m_ExpansionMenu.GetComponent<NavigationGroup>();
        }

        Open(0);
    }

    public void Close(int index)
    {
        if (index >= m_Animators.Length || m_Animators[index] == null)
            return;
            
        m_Animators[index].SetBool("Open", false);
        EventSystem.current.SetSelectedGameObject(m_ExpansionGroups[index].m_TriggerButton);
    }
    public void Open(int index)
    {
        if (index >= m_Animators.Length || m_Animators[index] == null)
            return;
            
        for (int i = 0; i < m_Animators.Length; ++i)
        {
            if (i == index)
                m_Animators[i].SetBool("Open", true);
            else
                m_Animators[i].SetBool("Open", false);
        }

        SetCurrentSelectedIndex(index);
        m_NavGroups[index].SelectFirstElement();
    }

    public void CloseCurrentSelected()
    {
        Close(m_CurrentSelectedIndex);
    }

    public void OpenCurrentSelected()
    {
        Open(m_CurrentSelectedIndex);
    }

    public void SetCurrentSelectedIndex(int index)
    {
        m_CurrentSelectedIndex = index;
    }
}
