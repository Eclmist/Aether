using UnityEngine;

public class AiMushroomAnimation : AiAnimation
{
    private enum AnimMovesParam
    {
        locomotion,
        attack1 = 1,
        attack2 = 2,
        attack3 = 2,
        attack4Start,
        attack4End,
        idleBreak,
        death,
        gotHit,
        goMonster,
        goStatue,
    }

    public override void Death()
    {
        m_Animator.SetTrigger(AnimMovesParam.death.ToString());
    }
    
    public override void TakenDamage()
    {
        m_Animator.SetTrigger(AnimMovesParam.gotHit.ToString());
    }

    public override void ReactToPlayer()
    {
        m_Animator.SetTrigger(AnimMovesParam.goStatue.ToString());
    }

    public override void GoInactive()
    {
        m_Animator.SetTrigger(AnimMovesParam.goMonster.ToString());

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
        AnimMovesParam [] temp = {AnimMovesParam.attack1, AnimMovesParam.attack2, AnimMovesParam.attack3};
        AnimMovesParam attack = temp[Random.Range(0, temp.Length)];
        m_Animator.SetTrigger(attack.ToString());
        return ((float) attack) / 2;
    }
}
