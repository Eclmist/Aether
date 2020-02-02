using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class FlagManager : MonoBehaviour
{
    public GameObject m_FlagIconToggler;
    private FlagIconToggler m_FlagToggle;
    public GameObject m_Flag;
    private bool m_IsFlagInPosession;
    // Start is called before the first frame update
    void Start()
    {
        AetherInput.GetPlayerActions().Fire.performed += HandleFlag;
        //m_FlagToggle = m_FlagIconToggler.GetComponent<FlagIconToggler>();
        //Debug.Log(m_FlagToggle == null);
    }
    
    void HandleFlag(InputAction.CallbackContext ctx)
    {
        ButtonControl button = ctx.control as ButtonControl;
        if (!button.wasPressedThisFrame)
            return;

        if (!m_IsFlagInPosession)
            return;

        m_IsFlagInPosession = false;
        //m_FlagToggle.ResetIcon();
        //Instantiate(m_Flag, transform.position+(transform.forward*2)+(transform.up), transform.rotation);
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
