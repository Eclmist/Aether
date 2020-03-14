using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RevealModifierVolume : MonoBehaviour
{
    [SerializeField]
    private float m_TargetRevealRadius = 1.5f;

    private void OnTriggerStay(Collider other)
    {
        RevealActor revealActor = other.GetComponent<RevealActor>();
        if (revealActor == null)
            return;

        revealActor.SetRadiusModifier(m_TargetRevealRadius);
    }
}
