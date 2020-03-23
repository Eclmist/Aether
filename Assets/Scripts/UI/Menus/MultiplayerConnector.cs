using UnityEngine;
using UnityEngine.UI;


public class MultiplayerConnector : MonoBehaviour
{
    [SerializeField]
    private InputField m_IPAddressField;

    [SerializeField]
    private InputField m_PortNumberField;
    public void Start()
    {
        if (m_IPAddressField != null)
            m_IPAddressField.text = MultiplayerManager.Instance.GetIPAddress();

        if (m_PortNumberField != null)
            m_PortNumberField.text = MultiplayerManager.Instance.GetPortNumber();
    }
    public void SendData() 
    {
        MultiplayerManager.Instance.SaveIPAddress(m_IPAddressField.text);
        MultiplayerManager.Instance.SavePortNumber(m_PortNumberField.text);
    }
}
