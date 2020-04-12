using UnityEngine;

public class AiPlantAnimation : AiAnimation
{
    private enum AnimMovesParam
    {
        locomotion,
        attack1 = 1,
        attack2 = 2,
        death = 3,
        gotHit,
        goPlant,
        goAlive
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
        m_Animator.SetTrigger(AnimMovesParam.goAlive.ToString());
        return (float) AnimMovesParam.goAlive;
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
        return ((float) attack) / 2;
    }
}
