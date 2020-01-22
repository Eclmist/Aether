using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class NavigationGroup : MonoBehaviour
{
    [SerializeField]
    private bool m_HorizontalWrapAround = true;

    [SerializeField]
    private bool m_VerticalWrapAround = true;

    [SerializeField]
    private bool m_UseVimStyleNextline = true;

    private GridLayoutGroup m_LayoutGroup;
    private int m_Rows = 0;
    private int m_Columns = 0;

    private Selectable[] m_NavigationElements;

    protected void Awake()
    {
        List<Selectable> childSelectables = new List<Selectable>();
        foreach (Transform firstLayerChildren in transform)
        {
            Selectable s = firstLayerChildren.GetComponent<Selectable>();
            if (s != null)
                childSelectables.Add(s);
        }

        m_NavigationElements = childSelectables.ToArray();
        m_LayoutGroup = GetComponent<GridLayoutGroup>();
    }

    protected void Start()
    {
        if (m_NavigationElements.Length <= 0)
            Destroy(this);

        switch(m_LayoutGroup.constraint)
        {
            // TODO: Support Fixed Row Count contraint when needed
            case GridLayoutGroup.Constraint.FixedColumnCount:
                m_Columns = m_LayoutGroup.constraintCount;
                m_Rows = (m_NavigationElements.Length + m_Columns - 1) / m_Columns;
                break;
            default:
                Debug.LogAssertion("Only Constraint.FixedColumnCount is supported by NavigationGroup!");
                Destroy(this);
                break;
        }

        SetupNavigation();
    }

    protected void SetupNavigation()
    {
        for (int i = 0; i < m_NavigationElements.Length; ++i)
        {
            int currentRowIndex = GetRowIndex(i);
            Navigation newNavigation = new Navigation();
            newNavigation.mode = Navigation.Mode.Explicit;

            newNavigation.selectOnLeft = m_NavigationElements[FindLeftElement(i)];
            newNavigation.selectOnRight = m_NavigationElements[FindRightElement(i)];
            newNavigation.selectOnUp = m_NavigationElements[FindUpElement(i)];
            newNavigation.selectOnDown = m_NavigationElements[FindDownElement(i)];

            m_NavigationElements[i].navigation = newNavigation;
        }
    }

    protected int FindLeftElement(int index)
    {
        int leftIndex = index - 1;

        // Check for underflow
        if (leftIndex < 0)
            leftIndex = m_HorizontalWrapAround ? m_NavigationElements.Length - 1 : 0;

        return leftIndex;
    }
    protected int FindRightElement(int index)
    {
        int rightIndex = index + 1;

        // Check for overflow
        if (rightIndex >= m_NavigationElements.Length)
            rightIndex = m_HorizontalWrapAround ? 0 : index;

        return rightIndex;
    }

    protected int FindUpElement(int index)
    {
        int upIndex = index - m_Columns;

        // Check for underflow
        if (upIndex < 0)
        {
            if (!m_VerticalWrapAround)
                return index;

            while (upIndex + m_Columns < m_NavigationElements.Length + m_Columns)
                upIndex += m_Columns;
        }

        // Check for overflow
        if (upIndex >= m_NavigationElements.Length)
        {
            if (m_UseVimStyleNextline && IsSameRow(upIndex, m_NavigationElements.Length - 1))
                upIndex = m_NavigationElements.Length - 1;
            else
                upIndex -= m_Columns;
        }

        return upIndex;
    }
    protected int FindDownElement(int index)
    {
        int downIndex = index + m_Columns;

        // Check for overFlow
        if (downIndex >= m_NavigationElements.Length)
        {
            if (m_UseVimStyleNextline && IsSameRow(downIndex, m_NavigationElements.Length - 1))
                return m_NavigationElements.Length - 1;

            if (!m_VerticalWrapAround)
                return index;

            while (downIndex - m_Columns >= 0)
                downIndex -= m_Columns;
        }

        return downIndex;
    }

    protected bool IsSameRow(int indexA, int indexB)
    {
        return GetRowIndex(indexA) == GetRowIndex(indexB);
    }

    protected int GetRowIndex(int index)
    {
        if (index < 0)
            return -1;

        return index / m_Columns;
    }

    public Selectable GetFirstElement()
    {
        return m_NavigationElements[0];
    }

    void Update()
    {
        
    }
}
