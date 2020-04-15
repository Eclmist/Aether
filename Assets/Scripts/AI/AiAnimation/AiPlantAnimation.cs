using UnityEngine;

public class AiPlantAnimation : AiAnimation
{
    private enum AnimMovesParam
    {
        locomotion,
        attack1,
        attack2,
        death,
        gotHit,
        goPlant,
        goAlive
    }

    private float GetTime(AnimMovesParam anim)
    {
        switch (anim)
        {
            case AnimMovesParam.attack1:
                return 1.0f;
            case AnimMovesParam.attack2:
                return 1.4f;
            case AnimMovesParam.death:
                return 3.4f;
            case AnimMovesParam.gotHit:
                return 0.15f;
            case AnimMovesParam.goPlant:
                return 0.7f;
            case AnimMovesParam.goAlive:
                return 0.7f;
            default:
                return 1f;
        }

    }
    public override float Death()
    {
        m_Animator.SetFloat("locomotion", 0);
        m_Animator.SetTrigger(AnimMovesParam.death.ToString());
        return GetTime(AnimMovesParam.death);
    }
    public override float TakenDamage()
    {
        m_Animator.SetTrigger(AnimMovesParam.gotHit.ToString());
        return GetTime(AnimMovesParam.gotHit);

    }
    
    public override float ReactToPlayer()
    {
        m_Animator.SetTrigger(AnimMovesParam.goAlive.ToString());
        return GetTime(AnimMovesParam.goAlive);
    }

    public override void GoInactive()
    {
        m_Animator.SetTrigger(AnimMovesParam.goPlant.ToString());

    }

    public override void Move(bool toMove)
    {
        if (toMove)
            m_Animator.SetFloat("locomotion", 0.55f); //hardcoded
        else
            m_Animator.SetFloat("locomotion", 0);
    }

    public override float RandomizeAttack()
    {
        AnimMovesParam [] temp = {AnimMovesParam.attack1, AnimMovesParam.attack2};
        AnimMovesParam attack = temp[Random.Range(0, temp.Length)];
        m_Animator.SetTrigger(attack.ToString());
        return GetTime(attack);
    }
}
