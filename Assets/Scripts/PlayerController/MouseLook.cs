using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Range(0, 200)]
    public float m_MouseSensitivity = 100;

    [SerializeField]
    private Transform m_Player = null;

    private float m_xRot = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseInput = AetherInput.GetPlayerActions().Look.ReadValue<Vector2>();
        float mouseX = mouseInput.x * m_MouseSensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * m_MouseSensitivity * Time.deltaTime;

        m_xRot -= mouseY;
        m_xRot = Mathf.Clamp(m_xRot, -90, 90);

        transform.localRotation = Quaternion.Euler(m_xRot, 0, 0);
        m_Player.Rotate(Vector3.up, mouseX);
    }
}
