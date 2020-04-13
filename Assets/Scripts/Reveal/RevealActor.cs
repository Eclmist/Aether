using UnityEngine;

public class RevealActor : MonoBehaviour
{
    [SerializeField]
    private float m_Radius = 5;
    private float m_RadiusModifier = 1;
    private float m_RadiusModifierTarget = 1;

    [SerializeField]
    private bool m_RevealObjectsByDistance = false;
    [SerializeField]
    private LayerMask m_ObjectLayerMask = new LayerMask();

    private RevealMode m_RevealMode = RevealMode.REVEALMODE_SHOW;

    private VisibilityManager.VisibilityModifier m_VisibilityModifier;

    private void Start()
    {
        m_VisibilityModifier = new VisibilityManager.VisibilityModifier();
        m_VisibilityModifier.m_Position = transform.position;
        m_VisibilityModifier.m_Radius = GetRadius();
        VisibilityManager.Instance.RegisterPersistentVisibility(m_VisibilityModifier);
    }

    // Update the terrain per vertex. 
    private void Update()
    {
        m_RadiusModifier = Mathf.Lerp(m_RadiusModifier, m_RadiusModifierTarget, Time.deltaTime);
        // Update Revealable Objects (the ones that fade from bottom to top in one go)

        m_VisibilityModifier.m_Position = transform.position;
        m_VisibilityModifier.m_Radius = GetRadius();

        switch (m_RevealMode)
        {
            case RevealMode.REVEALMODE_SHOW:
                m_VisibilityModifier.m_IsUnreveal = false;
                break;
            case RevealMode.REVEALMODE_HIDE:
                m_VisibilityModifier.m_IsUnreveal = true;
                break;
            default:
                break;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            m_VisibilityModifier.m_IsUnreveal = !m_VisibilityModifier.m_IsUnreveal;
        }

        if (m_RevealObjectsByDistance)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, GetRadius(), m_ObjectLayerMask);

            foreach (Collider c in colliders)
            {
                RevealableObject target = c.GetComponent<RevealableObject>();

                if (target == null)
                    continue;

                if (!m_VisibilityModifier.m_IsUnreveal)
                    target.Reveal();
                else
                    target.Hide();
            }
        }

        ResetRadiusModifier();
    }

    private float GetRadius()
    {
        return m_Radius * m_RadiusModifier;
    }

    public void SetRadiusModifier(float mod)
    {
        m_RadiusModifierTarget = Mathf.Max(mod, m_RadiusModifierTarget);
    }

    public void ResetRadiusModifier()
    {
        m_RadiusModifierTarget = 1;
    }

    public void SetRevealMode(RevealMode revealMode)
    {
        m_RevealMode = revealMode;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_Radius);
    }

    public enum RevealMode
    {
        REVEALMODE_SHOW, REVEALMODE_HIDE
    }
}
