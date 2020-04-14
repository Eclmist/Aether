using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Class destroys the gameobject once the effect is considered to be over.
 *  Place script on spell VFX
 */
public class SkillsDestroyer : MonoBehaviour
{
    [SerializeField]
    private float m_TimeOfDestruction = 7.0f;

    void Start()
    {
        Destroy(gameObject, m_TimeOfDestruction);
    }
}
