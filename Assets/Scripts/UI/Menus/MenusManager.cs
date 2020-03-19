using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenusManager : Singleton<MenusManager>
{
    [SerializeField]
    private TriggerableAnimator[] m_Animators;

    public void TriggerAnimator(int index)
    {
        if (m_Animators == null || index >= m_Animators.Length)
            return;

        TriggerableAnimator animator = m_Animators[index];
        animator.TriggerAnimation();
    }

    public bool GetTriggerBool(int index)
    {
         if (m_Animators == null || index >= m_Animators.Length)
            throw new UnityException("Should not call on null or out of bounds");

        TriggerableAnimator animator = m_Animators[index];
        return animator.GetTriggerBool();
    }
}
