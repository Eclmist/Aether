using UnityEngine;

/*
 * Only for AiActors which implement attacker
 */
public class AttackPlayer : AiStateBehaviour
{
    [SerializeField] private float m_AttackInterval = 2f;
    [SerializeField] [Range(0, 50)]
    private float m_AttackRange = 3f;
    public override void Init()
    {
        
    }

    public override void Update()
    {
        Attacker attacker = m_AiActor as Attacker;

        if (attacker == null)
        {
            Debug.LogWarning("Trying to attack without implementing attacker");
            return;
        }
        
        if (m_AttackRange > Vector3.Distance(m_NearestPlayer.position, m_AiActor.gameObject.transform.position))
        {
            attacker.Attack(m_AttackInterval);
        }
    }
}
