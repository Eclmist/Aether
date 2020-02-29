using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Courtesy of Dylan Wolf: https://www.dylanwolf.com/2018/11/24/stupid-unity-ui-navigation-tricks/
 */
public class PreventDeselectionGroup : MonoBehaviour
{
    EventSystem m_EventSystem;
    private void Start()
    {
        m_EventSystem = EventSystem.current;
    }

    GameObject m_SelectedObj;

    private void Update()
    {
        if (m_EventSystem.currentSelectedGameObject != null && m_EventSystem.currentSelectedGameObject != m_SelectedObj)
            m_SelectedObj = m_EventSystem.currentSelectedGameObject;
        else if (m_SelectedObj != null && m_EventSystem.currentSelectedGameObject == null)
            m_EventSystem.SetSelectedGameObject(m_SelectedObj);
    }
}
