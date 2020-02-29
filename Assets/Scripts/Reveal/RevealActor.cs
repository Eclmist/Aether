using UnityEngine;

public class RevealActor : MonoBehaviour
{
    [SerializeField]
    private float m_Radius = 5;

    [SerializeField]
    private float m_RadiusModifierForVisibilityMgr = 0.5f;

    [SerializeField]
    private LayerMask m_ObjectLayerMask = new LayerMask();

    private RevealMode m_RevealMode = RevealMode.REVEALMODE_SHOW;

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

        m_VisibilityModifier.m_Position = transform.position;
        m_VisibilityModifier.m_Radius = m_Radius;

        switch (m_RevealMode)
        {
            case RevealMode.REVEALMODE_SHOW:
                m_VisibilityModifier.m_TargetVisibility = 1;

                foreach (Collider c in colliders)
                {
                    RevealableObject target = c.GetComponent<RevealableObject>();
                    if (target == null)
                        continue;

                    target.Reveal();
                }
                break;
            case RevealMode.REVEALMODE_HIDE:
                m_VisibilityModifier.m_TargetVisibility = 0;

                foreach (Collider c in colliders)
                {
                    RevealableObject target = c.GetComponent<RevealableObject>();
                    if (target == null)
                        continue;

                    target.Hide();
                }
                break;
            default:
                Debug.LogWarning("Should not be entering default case in RevealActor Update");
                break;
        }
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
