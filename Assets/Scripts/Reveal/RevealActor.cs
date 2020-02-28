using UnityEngine;

public class RevealActor : MonoBehaviour
{
    [SerializeField]
    private float m_Radius = 5;

    [SerializeField]
    private float m_RadiusModifierForVisibilityMgr = 0.5f;

    [SerializeField]
    private LayerMask m_ObjectLayerMask = new LayerMask();

    private RevealMode m_RevealMode = RevealMode.SHOW;

    private VisibilityManager.VisibilityModifier m_VisibilityModifier;

    private void Start()
    {
        m_VisibilityModifier = new VisibilityManager.VisibilityModifier();
        m_VisibilityModifier.m_Position = transform.position;
        m_VisibilityModifier.m_Radius = m_Radius * m_RadiusModifierForVisibilityMgr;
        m_VisibilityModifier.m_TargetVisibility = 1;
        VisibilityManager.Instance.RegisterPersistentVisibility(m_VisibilityModifier);
    }

    // Update the terrain per vertex. 
    private void Update()
    {
        // Update Revealable Objects (the ones that fade from bottom to top in one go)
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_Radius, m_ObjectLayerMask);

        foreach (Collider c in colliders)
        {
            RevealableObject target = c.GetComponent<RevealableObject>();
            
            if (target == null)
                continue;

            if (m_RevealMode == RevealMode.SHOW)
                target.Reveal();
            else
                target.Hide();
        }

        m_VisibilityModifier.m_Position = transform.position;
        m_VisibilityModifier.m_Radius = m_Radius;
        m_VisibilityModifier.m_TargetVisibility = m_RevealMode == RevealMode.SHOW ? 1 : 0;
    }

    public void SetRevealMode(RevealMode revealMode)
    {
        m_RevealMode = revealMode;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_Radius);
    }
}
