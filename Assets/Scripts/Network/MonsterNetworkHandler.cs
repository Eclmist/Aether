using System.Collections;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;

[RequireComponent(typeof(AiMonster), typeof(HealthHandler))]
public class MonsterNetworkHandler : MonsterObjectBehavior
{
    private AiMonster m_aiMonster;
    private HealthHandler m_healthHandler;

    private void Start()
    {
        m_aiMonster = GetComponent<AiMonster>();
        m_healthHandler = GetComponent<HealthHandler>();
        if (m_aiMonster == null || m_healthHandler == null)
        {
            Debug.LogError("No health handler and ai monster attached " + this);
        }
        if (networkObject == null)
        {
            this.enabled = false;
            return;
        }
    }
    
    private void Update()
    {

        if (networkObject.IsOwner)
        {
            // If we are the owner of the object we should send the new position
            // and rotation across the network for receivers to move to in the above code
            networkObject.position = transform.position;
            networkObject.rotation = transform.rotation;
            networkObject.isDead = m_aiMonster.IsDead;
            networkObject.health = m_healthHandler.GetHealth();

        }
        else
        {
            transform.position = networkObject.position;
            transform.rotation = networkObject.rotation;
            if (networkObject.isDead)
            {
                m_aiMonster.OnDeath();
            }
            m_healthHandler.SetHealth(networkObject.health);
        }

    }

}
