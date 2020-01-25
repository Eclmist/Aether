using UnityEngine;

[RequireComponent(typeof(PlayerNetworkHandler))]
public class PlayerNetworkMovement : MonoBehaviour
{
    private PlayerNetworkHandler m_PlayerNetworkHandler;

    void Start()
    {
        m_PlayerNetworkHandler = GetComponent<PlayerNetworkHandler>();
    }

    void Update()
    {
        if (m_PlayerNetworkHandler.networkObject == null)
            return;

        transform.position = m_PlayerNetworkHandler.networkObject.position;
    }
}
