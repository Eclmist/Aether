
/*
 * Requires "isSafe" parameter in the animator. Will flee from player when nearby.
 */
public class Idle : AiStateBehaviour
{
    public override void Init()
    { 
        m_AiActor.SetInactive();
    }

    public override void OnExit()
    {
        m_AiActor.SetActive();
    }

}
