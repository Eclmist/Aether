using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizeUI : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_ExpansionMenus;

    private Animator[] m_Animators;

    private int m_CurrentSelectedIndex = 0;

    // Start is called before the first frame update
    protected void Start()
    {
        m_Animators = new Animator[m_ExpansionMenus.Length];

        for (int i = 0; i < m_ExpansionMenus.Length; ++i)
            m_Animators[i] = m_ExpansionMenus[i].GetComponent<Animator>();
    }

    protected void Update()
    {
        // if (Input.GetButtonDown("Cancel"))
        //     Close(m_CurrentSelectedIndex);

        // if (Input.GetButtonDown("Submit"))
        //     Open(m_CurrentSelectedIndex);
    }

    public void Close(int index)
    {
        if (index >= m_Animators.Length || m_Animators[index] == null)
            return;
            
        m_Animators[index].SetBool("Open", false);
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

        m_CurrentSelectedIndex = index;
    }

}
