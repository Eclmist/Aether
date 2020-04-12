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
    private Color m_KeyboardIconColor;

    [SerializeField]
    private Sprite m_PS4ControllerIcon;

    [SerializeField]
    private Color m_PS4IconColor;

    [SerializeField]
    private Button m_Button;
    // Start is called before the first frame update
    void Start()
    {
        PlatformManager.Instance.PlatformChanged += OnPlatformChanged;
    }

    private void SetMouseFriendly(bool flag)
    {
        if (m_Button == null)
            return;

        m_Button.interactable = flag;
    }

    private void OnPlatformChanged(PlatformType platformType)
    {
        if (m_Icon == null || m_KeyboardMouseIcon == null || m_PS4ControllerIcon == null)
            return;

        switch(platformType)
        {
            case PlatformType.PS4_CONTROLLER:
                ChangeIcon(m_PS4ControllerIcon, m_PS4IconColor);
                SetMouseFriendly(false);
                break;
            case PlatformType.KEYBOARD_MOUSE:
                ChangeIcon(m_KeyboardMouseIcon, m_KeyboardIconColor);
                SetMouseFriendly(true);
                break;
            default: 
                break;

        }
    }
    private void ChangeIcon(Sprite sprite, Color color)
    {
        if (m_Icon == null || sprite == null)
            return;

        m_Icon.sprite = sprite;
        m_Icon.color = color;
        
    }

    private void OnDestroy()
    {
        if (PlatformManager.HasInstance)
            PlatformManager.Instance.PlatformChanged -= OnPlatformChanged;
    }
}
