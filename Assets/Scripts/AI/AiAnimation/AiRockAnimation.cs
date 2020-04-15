using UnityEngine;

public class AiRockAnimation : AiAnimation
{
    private enum AnimMovesParam: long
    {
        locomotion,
        attack1A,
        attack1B,
        attack2,
        death,
        gotHit,
        idleToRubble,
        rubbleToIdle,
    }

    private float GetTime(AnimMovesParam anim)
    {
        switch (anim)
        {
            case AnimMovesParam.attack1A:
                return 1.0f;
            case AnimMovesParam.attack1B:
                return 1.5f;
            case AnimMovesParam.attack2:
                return 1.3f;
            case AnimMovesParam.death:
                return 3.4f;
            case AnimMovesParam.gotHit:
                return 0.3f;
            case AnimMovesParam.idleToRubble:
                return 2.7f;
            case AnimMovesParam.rubbleToIdle:
                return 3.8f;
            default:
                return 1f;
        }

    }
    public override float Death()
    {
        m_Animator.SetTrigger(AnimMovesParam.death.ToString());
        return GetTime(AnimMovesParam.death);
    }
    
    public override void TakenDamage()
    {
        m_Animator.SetTrigger(AnimMovesParam.gotHit.ToString());
    }

    public override float ReactToPlayer()
    {
        m_Animator.SetTrigger(AnimMovesParam.rubbleToIdle.ToString());
        return GetTime(AnimMovesParam.rubbleToIdle);
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
        AnimMovesParam[] temp = {AnimMovesParam.attack1A, AnimMovesParam.attack1B, AnimMovesParam.attack2};
        int range = Random.Range(0, temp.Length);
        
        AnimMovesParam attack = temp[range];
        
        m_Animator.SetTrigger(attack.ToString());
        return GetTime(attack);
    }
}
