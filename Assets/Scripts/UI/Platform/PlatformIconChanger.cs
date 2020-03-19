using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformIconChanger : MonoBehaviour
{
    [SerializeField]
    private Image m_Icon;

    [SerializeField]
    private Sprite m_KeyboardMouseIcon;

    [SerializeField]
    private Sprite m_PS4ControllerIcon;
    // Start is called before the first frame update
    void Start()
    {
        m_Icon.sprite = m_KeyboardMouseIcon;
    }

    // Update is called once per frame
    void Update()
    {
        SwitchPlatformIcon();
    }

    private void SwitchPlatformIcon()
    {
        PlatformType platformType = PlatformManager.Instance.GetPlatformType();

        if (m_Icon == null || m_KeyboardMouseIcon == null || m_PS4ControllerIcon == null)
            return;

        switch(platformType)
        {
            case PlatformType.PS4_CONTROLLER:
                ChangeIcon(m_PS4ControllerIcon);
                break;
            case PlatformType.KEYBOARD_MOUSE:
                ChangeIcon(m_KeyboardMouseIcon);
                break;
            default: 
                break;

        }
    }

    private void ChangeIcon(Sprite sprite)
    {
        if (m_Icon == null || sprite == null)
            return;

        m_Icon.sprite = sprite;
        
    }
}
