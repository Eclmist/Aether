using System.Collections;
using UnityEngine;

public class AiWormAnimation : AiAnimation
{
    private enum AnimMovesParam
    {
        attack1,
        attack2,
        attack4Start,
        attack4End,
        idleBreak,
        death,
        gotHitBody,
        appear,
        disappear,
    }

    private float GetTime(AnimMovesParam anim)
    {
        switch (anim)
        {
            case AnimMovesParam.attack1:
                return 1.0f;
            case AnimMovesParam.attack2:
                return 1.5f;
            case AnimMovesParam.death:
                return 3.4f;
            case AnimMovesParam.gotHitBody:
                return 0.3f;
            case AnimMovesParam.appear:
                return 2.7f;
            case AnimMovesParam.disappear:
                return 4f;
            default:
                return 1f;
        }

    }
    public override float Death()
    {
        m_Animator.SetTrigger(AnimMovesParam.death.ToString());
        return GetTime(AnimMovesParam.death);
    }
    
    public override float TakenDamage()
    {
        m_Animator.SetTrigger(AnimMovesParam.gotHitBody.ToString());
        return GetTime(AnimMovesParam.gotHitBody);

    }

    public override float ReactToPlayer()
    {
        m_Animator.SetTrigger(AnimMovesParam.appear.ToString());
        return GetTime(AnimMovesParam.appear);
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

    public override float RandomizeAttack(out string attackName)
    {
        AnimMovesParam[] temp = {AnimMovesParam.attack1, AnimMovesParam.attack2};
        AnimMovesParam attack = temp[Random.Range(0, temp.Length)];
        attackName = attack.ToString();
        StartCoroutine(Attack(ReactToPlayer(), attack));
       
        return GetTime(attack);
    }

    IEnumerator Attack(float delay, AnimMovesParam attack)
    {
        yield return new WaitForSeconds(delay);
       
        m_Animator.SetTrigger(attack.ToString());
    }

    public override void SetAttackTrigger(string name)
    {
        StartCoroutine(Attack(ReactToPlayer(), (AnimMovesParam)System.Enum.Parse(typeof(AnimMovesParam), name)));
    }
}
