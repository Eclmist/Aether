using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;

public class MeteorNetwork : SkillsBehavior
{
    [SerializeField]
    private float m_TimeOfDestruction = 7.0f;

    private float m_Timer;

    // Start is called before the first frame update
    void Start()
    {
        m_Timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Timer >= m_TimeOfDestruction)
        {
            Destroy(gameObject);
        }

        m_Timer += Time.deltaTime;
    }
}
