using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlatformIconChanger : MonoBehaviour
{
    private bool m_IsMouseFriendly;
    [SerializeField]
    private Image m_Icon;

    [SerializeField]
    private Text m_Text;

    [SerializeField]
    private Sprite m_KeyboardMouseIcon;
    
    [SerializeField]
    private string m_KeyboardText;

    [SerializeField]
    private Color m_KeyboardColor;

    [SerializeField]
    private Sprite m_PS4ControllerIcon;

    [SerializeField]
    private string m_PS4Text;

    [SerializeField]
    private Color m_PS4Color;

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
                ChangeIcon(m_PS4ControllerIcon, m_PS4Color, m_PS4Text);
                SetMouseFriendly(false);
                break;
            case PlatformType.KEYBOARD_MOUSE:
                ChangeIcon(m_KeyboardMouseIcon, m_KeyboardColor, m_KeyboardText);
                SetMouseFriendly(true);
                break;
            default: 
                break;

        }
    }
    private void ChangeIcon(Sprite sprite, Color color, string text)
    {
        if (m_Icon == null || sprite == null || color == null || text  == null)
            return;

        m_Icon.sprite = sprite;
        m_Icon.color = color;
        m_Text.text = text;
    }

    private void OnDestroy()
    {
        if (PlatformManager.HasInstance)
            PlatformManager.Instance.PlatformChanged -= OnPlatformChanged;
    }
}
