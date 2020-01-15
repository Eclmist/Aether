using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [Range(0, 10)]
    private float m_MoveSpeed = 6;

    [SerializeField]
    private Transform m_GroundCheck = null;

    [SerializeField]
    private LayerMask m_LayerMask = new LayerMask();

    [SerializeField]
    private float m_Gravity = -9.8f;

    [SerializeField]
    private float m_JumpHeight = 1.3f;

    private CharacterController m_CharacterController;

    private Vector3 m_Velocity;

    private float m_PlayerHeight;


    // Start is called before the first frame update
    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_PlayerHeight = m_CharacterController.height;
    }

    // Update is called once per frame
    void Update()
    {
        bool isGrounded = Physics.CheckSphere(m_GroundCheck.position, 0.2f, m_LayerMask);

        if (isGrounded && m_Velocity.y < 0)
            m_Velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = Camera.main.transform.right * x + Camera.main.transform.forward * z;
        move *= Time.deltaTime * m_MoveSpeed;

        m_CharacterController.Move(move);

        if (Input.GetButtonDown("Jump") && isGrounded)
            m_Velocity.y = Mathf.Sqrt(m_JumpHeight * -2 * m_Gravity);

        m_Velocity.y += m_Gravity * Time.deltaTime;
        m_CharacterController.Move(m_Velocity * Time.deltaTime);


        if (Input.GetKey(KeyCode.LeftControl))
            m_CharacterController.height = m_PlayerHeight / 2;
        else
            m_CharacterController.height = m_PlayerHeight;
    }
}
