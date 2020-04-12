using UnityEngine;

public class AiMushroomAnimation : AiAnimation
{
    private enum AnimMovesParam
    {
        locomotion,
        attack1 = 1,
        attack2 = 2,
        attack3 = 2,
        death = 3,
        gotHit,
        goMonster,
        goStatue = 2,
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
        m_Animator.SetTrigger(AnimMovesParam.goStatue.ToString());
        return (float) AnimMovesParam.goStatue;
    }

    public override void GoInactive()
    {
        m_Animator.SetTrigger(AnimMovesParam.goMonster.ToString());
    }

    public override void Move(bool toMove)
    {
        if (toMove)
            m_Animator.SetFloat("locomotion", 0.80f); //hardcoded
        else
            m_Animator.SetFloat("locomotion", 0);
    }

    public override float RandomizeAttack()
    {
        AnimMovesParam [] temp = {AnimMovesParam.attack1, AnimMovesParam.attack2, AnimMovesParam.attack3};
        AnimMovesParam attack = temp[Random.Range(0, temp.Length)];
        Debug.Log(attack);
        m_Animator.SetTrigger(attack.ToString());
        return ((float) attack) / 2;
    }
}
