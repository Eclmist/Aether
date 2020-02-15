using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerNetworkMovement : MonoBehaviour
{
    private Player m_Player;

    void Start()
    {
        m_Player = GetComponent<Player>();
    }

    void Update()
    {
        if (m_Player.networkObject == null)
            return;

        transform.position = m_Player.networkObject.position;
    }
}
