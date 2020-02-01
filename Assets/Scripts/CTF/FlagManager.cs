using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class FlagManager : MonoBehaviour
{
    private FlagIconToggler m_FlagIconToggler;
    public GameObject m_Flag;
    private bool m_IsFlagInPosession;
    // Start is called before the first frame update
    void Start()
    {
        AetherInput.GetPlayerActions().Fire.performed += HandleFlag;
        m_FlagIconToggler = gameObject.GetComponentInChildren<FlagIconToggler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsFlagInPosession)
        {
            m_FlagIconToggler.SetIcon();
        }
    }

    void HandleFlag(InputAction.CallbackContext ctx)
    {
        ButtonControl button = ctx.control as ButtonControl;
        if (!button.wasPressedThisFrame)
            return;

        if (!m_IsFlagInPosession)
            return;

        m_IsFlagInPosession = false;
        m_FlagIconToggler.ResetIcon();
        Instantiate(m_Flag, transform.position+(transform.forward*2)+(transform.up), transform.rotation);
    }

    public void SetBool(bool boolean)
    {
        m_IsFlagInPosession = boolean;
    }

    public bool CheckIfFlagInPosession()
    {
        return m_IsFlagInPosession;
    }
}
