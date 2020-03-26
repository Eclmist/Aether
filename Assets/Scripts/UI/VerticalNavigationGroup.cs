using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class VerticalNavigationGroup : MonoBehaviour
{
    [SerializeField]
    private bool m_VerticalWrapAround = true;

    [SerializeField]
    private bool m_ScrollOnNavigate = false;

    private VerticalLayoutGroup m_LayoutGroup;
    private Selectable[] m_NavigationElements;

    protected void Awake()
    {
        List<Selectable> childSelectables = new List<Selectable>();
        foreach (Transform firstLayerChildren in transform)
        {
            Selectable s = firstLayerChildren.GetComponent<Selectable>();
            if (s == null)
                continue;
            if (!s.interactable)
                continue;

            childSelectables.Add(s);
        }

        m_NavigationElements = childSelectables.ToArray();
        m_LayoutGroup = GetComponent<VerticalLayoutGroup>();
    }

    protected void Start()
    {
        if (m_NavigationElements.Length <= 0)
            Destroy(this);

        SetupNavigation();
    }

    protected void SetupNavigation()
    {
        for (int i = 0; i < m_NavigationElements.Length; ++i)
        {
            Navigation newNavigation = new Navigation();
            newNavigation.mode = Navigation.Mode.Explicit;

            newNavigation.selectOnUp = m_NavigationElements[FindNextElement(i, -1)];
            newNavigation.selectOnDown = m_NavigationElements[FindNextElement(i, 1)];

            m_NavigationElements[i].navigation = newNavigation;
        }
    }

    protected int FindNextElement(int index, int indexMod)
    {
        int newIndex = index + indexMod;

        if (newIndex < 0 && m_VerticalWrapAround)
            newIndex = m_NavigationElements.Length - 1;

        if (newIndex >= m_NavigationElements.Length && m_VerticalWrapAround)
            newIndex = 0;

        return Mathf.Clamp(newIndex, 0, m_NavigationElements.Length);
    }

    public Selectable GetFirstElement()
    {
        return m_NavigationElements[0];
    }

    public void SelectFirstElement()
    {
        EventSystem.current.SetSelectedGameObject(GetFirstElement().gameObject);
    }
}
