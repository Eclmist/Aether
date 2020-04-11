using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormAttack : AiStateBehaviour
{
    [SerializeField] private float m_AttackInterval = 2f;
    [SerializeField] [Range(0, 50)] private float m_AttackRange = 3f;


    private Attacker m_Attacker;

    public override void Init()
    {
        m_Attacker = m_AiActor as Attacker;
    }

    public override void Update()
    {
    }

    public override void CoroutineUpdate()
    {
        if (m_Attacker == null)
        {
            Debug.LogWarning("Trying to attack without implementing attacker");
            return;
        }

        if (m_AttackRange > Vector3.Distance(m_NearestPlayer.position, m_AiActor.gameObject.transform.position))
        {
            m_Attacker.Attack(m_AttackInterval);
        }
        else
        {
            m_StateAnimator.SetBool("nearPlayer", false);
        }
    }
}
