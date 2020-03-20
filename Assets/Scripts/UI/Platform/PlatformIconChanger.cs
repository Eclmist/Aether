using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlatformIconChanger : MonoBehaviour
{
    private bool m_IsMouseFriendly;
    [SerializeField]
    private Image m_Icon;

    [SerializeField]
    private Sprite m_KeyboardMouseIcon;

    [SerializeField]
    private Sprite m_PS4ControllerIcon;

    [SerializeField]
    private Button m_Button;
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

    private void SetMouseFriendly(bool flag)
    {
        if (m_Button == null)
            return;

        m_Button.interactable = flag;
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
                SetMouseFriendly(false);
                break;
            case PlatformType.KEYBOARD_MOUSE:
                ChangeIcon(m_KeyboardMouseIcon);
                SetMouseFriendly(true);
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
