using UnityEngine;

/*
 * Used for creating ai animation for monsters.
 */
[RequireComponent(typeof(Animator))]
public abstract class AiAnimation : MonoBehaviour
{
    protected Animator m_Animator;
    
    private void Awake()
    {
        m_Animator = gameObject.GetComponent<Animator>();
    }

    public abstract void ReactToPlayer();
    
    public abstract void GoInactive();
    public abstract void Move(bool toMove);
    public abstract float RandomizeAttack();

    public abstract void TakenDamage();
    public abstract void Death();
}
