using UnityEngine;

public class AiMushroomAnimation : AiAnimation
{
    private enum AnimMovesParam
    {
        locomotion,
        attack1,
        attack2,
        attack3,
        death,
        gotHit,
        goMonster,
        goStatue,
    }

    private float GetTime(AnimMovesParam anim)
    {
        switch (anim)
        {
            case AnimMovesParam.attack1:
                return 1.0f;
            case AnimMovesParam.attack2:
                return 1.3f;
             case AnimMovesParam.attack3:
                 return 1.6f;
            case AnimMovesParam.death:
                return 1.4f;
            case AnimMovesParam.gotHit:
                return 0.15f;
            case AnimMovesParam.goMonster:
                return 0.2f;
            case AnimMovesParam.goStatue:
                return 3f;
            default:
                return 1f;
        }

    }
    public override float Death()
    {
        m_Animator.SetFloat("locomotion", 0);
        m_Animator.SetBool(AnimMovesParam.death.ToString(), true);
        return GetTime(AnimMovesParam.death);
    }
    
    public override float TakenDamage()
    {
        m_Animator.SetTrigger(AnimMovesParam.gotHit.ToString());
        return GetTime(AnimMovesParam.gotHit);
    }

    public override float ReactToPlayer()
    {
        m_Animator.SetTrigger(AnimMovesParam.goMonster.ToString());
        return GetTime(AnimMovesParam.goMonster);
    }

    public override void GoInactive()
    {
        m_Animator.SetTrigger(AnimMovesParam.goStatue.ToString());
    }

    public override void Move(bool toMove)
    {
        if (toMove)
            m_Animator.SetFloat("locomotion", 0.80f); //hardcoded
        else
            m_Animator.SetFloat("locomotion", 0);
    }

    public override float RandomizeAttack(out string attackName)
    {
        AnimMovesParam [] temp = {AnimMovesParam.attack1, AnimMovesParam.attack2, AnimMovesParam.attack3};
        AnimMovesParam attack = temp[Random.Range(0, temp.Length)];
        attackName = attack.ToString();
        m_Animator.SetTrigger(attack.ToString());
        return GetTime(attack);
    }

    public override void SetAttackTrigger(string name)
    {
        m_Animator.SetTrigger(name);
    }
}
