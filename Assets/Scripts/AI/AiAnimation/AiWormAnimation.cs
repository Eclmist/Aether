using System.Collections;
using UnityEngine;

public class AiWormAnimation : AiAnimation
{
    private enum AnimMovesParam
    {
        attack1 = 1,
        attack2 = 2,
        attack4Start,
        attack4End,
        idleBreak,
        death =3,
        gotHitBody,
        appear = 2,
        disappear,
    }

    public override float Death()
    {
        m_Animator.SetTrigger(AnimMovesParam.death.ToString());
        return (float) AnimMovesParam.death;
    }
    
    public override void TakenDamage()
    {
        m_Animator.SetTrigger(AnimMovesParam.gotHitBody.ToString());
    }

    public override float ReactToPlayer()
    {
        m_Animator.SetTrigger(AnimMovesParam.appear.ToString());
        return (float) AnimMovesParam.appear;
    }

    public override void GoInactive()
    {
        m_Animator.SetTrigger(AnimMovesParam.disappear.ToString());
    }

    public override void Move(bool toMove)
    {
        if (toMove)
            m_Animator.SetTrigger(AnimMovesParam.disappear.ToString());
        else
            m_Animator.SetTrigger(AnimMovesParam.appear.ToString());
    }

    public override float RandomizeAttack()
    {
        AnimMovesParam[] temp = {AnimMovesParam.attack1, AnimMovesParam.attack2};
        AnimMovesParam attack = temp[Random.Range(0, temp.Length)];
        StartCoroutine(Attack(ReactToPlayer(), attack));
       
        return ((float) attack) / 2;
    }

    IEnumerator Attack(float delay, AnimMovesParam attack)
    {
        yield return new WaitForSeconds(delay);
       
        m_Animator.SetTrigger(attack.ToString());
    }
}
