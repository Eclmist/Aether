using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationLookat : MonoBehaviour
{
    public enum LookAtType
    {
        LOOKAT_CAMERADIR,
        LOOKAT_MOUSEPOS,
        LOOKAT_NETWORKED
    }

    [SerializeField]
    private LookAtType m_LookAtType = LookAtType.LOOKAT_CAMERADIR;

    [SerializeField]
    private bool m_LookatDamping = false;

    [SerializeField]
    [Range(0, 1)]
    private float m_Weight = 0.6f;

    [SerializeField]
    [Range(0, 1)]
    private float m_WeightBody = 0.1f;

    [SerializeField]
    [Range(0, 1)]
    private float m_WeightHead = 1.0f;

    [SerializeField]
    [Range(0, 1)]
    private float m_WeightEye = 0.8f;

    [SerializeField]
    [Range(0, 1)]
    private float m_WeightClamp = 0.6f;

    [SerializeField]
    [Range(0, 20)]
    private float m_Distance = 10;

    [SerializeField]
    [Range(0, 10)]
    private float m_WeightDampingSpeed = 0.1f;
    
    [SerializeField]
    [Range(0, 1)]
    private float m_DisabledWeight = 0.2f;

    [SerializeField]
    private Transform m_EyePosition = null;

    private Animator m_Animator;

    private float m_LookAtWeightCache = 1;

    private Vector3 m_MousePosDampingCache;

    private Vector3 m_LookatDirection;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_LookAtWeightCache = m_Weight;
    }

    void OnAnimatorIK()
    {

        // Find a better way to do this
        if (m_Animator.enabled == false ||
            m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Landing") ||
            m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            m_LookAtWeightCache = Mathf.Lerp(m_LookAtWeightCache, m_DisabledWeight, Time.deltaTime * m_WeightDampingSpeed);
        }
        else
        {
            m_LookAtWeightCache = Mathf.Lerp(m_LookAtWeightCache, m_Weight, Time.deltaTime * m_WeightDampingSpeed);
        }

        switch (m_LookAtType)
        {
            case LookAtType.LOOKAT_CAMERADIR:
                m_LookatDirection = m_EyePosition.position + Camera.main.transform.forward * m_Distance;
                break;
            case LookAtType.LOOKAT_MOUSEPOS:
                // Mouse
                Vector3 mouse = Input.mousePosition;

                // Handle Gamepad if any
                Gamepad currentGamepad = Gamepad.current;   
                if (currentGamepad != null)
                {
                    Vector2 rightStick = currentGamepad.rightStick.ReadValue();
                    if (rightStick.magnitude > 0)
                    {
                        mouse = rightStick;
                        mouse.x = (mouse.x + 1) * (Screen.width / 2);
                        mouse.y = (mouse.y + 1) * (Screen.height / 2);
                    }
                }

                mouse.z = 3;

                // Do damping if necessary
                if (m_LookatDamping)
                {
                    m_MousePosDampingCache = Vector3.Lerp(m_MousePosDampingCache, mouse, Time.deltaTime * 10);
                    mouse = m_MousePosDampingCache;
                }
                   
                Vector3 dir = Camera.main.ScreenToWorldPoint(mouse) - m_EyePosition.position;
                m_LookatDirection = m_EyePosition.position + dir.normalized * m_Distance;
                break;
            default:
                return;
        }

        // If looking behind player
        if (Vector3.Dot(transform.forward, (m_LookatDirection - transform.position).normalized) < -0.4f)
            m_LookAtWeightCache = Mathf.Lerp(m_LookAtWeightCache, m_DisabledWeight, Time.deltaTime * m_WeightDampingSpeed);

        m_Animator.SetLookAtWeight(m_LookAtWeightCache, m_WeightBody, m_WeightHead, m_WeightEye, m_WeightClamp);
        m_Animator.SetLookAtPosition(m_LookatDirection);
    }

    public Vector3 GetLookatDirection()
    {
        return m_LookatDirection;
    }

    public void SetLookatDirection(Vector3 lookatDirection)
    {
        m_LookatDirection = lookatDirection;
    }

    public void SetLookatType(LookAtType type)
    {
        m_LookAtType = type;
    }

    private void OnDrawGizmos()
    {
        Vector3 lookat = Camera.main.transform.forward;
        Gizmos.DrawLine(m_EyePosition.position, m_EyePosition.position + lookat * m_Distance);
        Gizmos.DrawSphere(m_EyePosition.position + lookat * m_Distance, 0.5f);
    }
}
