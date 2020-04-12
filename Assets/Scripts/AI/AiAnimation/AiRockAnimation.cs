using UnityEngine;

public class AiRockAnimation : AiAnimation
{
    private enum AnimMovesParam
    {
        locomotion,
        attack1A = 1,
        attack1B = 2,
        attack2 = 2,
        death = 3,
        gotHit,
        idleToRubble,
        rubbleToIdle,
    }

    public override float Death()
    {
        m_Animator.SetTrigger(AnimMovesParam.death.ToString());
        return (float) AnimMovesParam.death;
    }
    
    public override void TakenDamage()
    {
        m_Animator.SetTrigger(AnimMovesParam.gotHit.ToString());
    }

    public override float ReactToPlayer()
    {
        m_Animator.SetTrigger(AnimMovesParam.rubbleToIdle.ToString());
        return (float) AnimMovesParam.rubbleToIdle;
    }

    public override void GoInactive()
    {
        m_Animator.SetTrigger(AnimMovesParam.idleToRubble.ToString());

    }

    public override void Move(bool toMove)
    {
        if (toMove)
            m_Animator.SetFloat("locomotion", 0.85f); //hardcoded
        else
            m_Animator.SetFloat("locomotion", 0);
    }

    public override float RandomizeAttack()
    {
        AnimMovesParam [] temp = {AnimMovesParam.attack1A, AnimMovesParam.attack1B, AnimMovesParam.attack2};
        AnimMovesParam attack = temp[Random.Range(0, temp.Length)];
        m_Animator.SetTrigger(attack.ToString());
        return ((float) attack) / 2;
    }
}
