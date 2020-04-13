using System.Collections;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;

public class MonsterNetworkHandler : MonsterObjectBehavior
{
    private AiMonster m_aiMonster;

    private void Start()
    {
        m_aiMonster = GetComponent<AiMonster>();
    }
    private void Update()
    {
        if (networkObject == null)
        {
            Debug.LogWarning("No monster network object found on " + this);
            return;
        }

        if (networkObject.IsOwner)
        {
            // If we are the owner of the object we should send the new position
            // and rotation across the network for receivers to move to in the above code
            networkObject.position = transform.position;
            networkObject.rotation = transform.rotation;
            networkObject.isDead = m_aiMonster.IsDead;

        }
        else
        {
            transform.position = networkObject.position;
            transform.rotation = networkObject.rotation;
            m_aiMonster.IsDead = networkObject.isDead;
            m_aiMonster.OnDeath();
        }

    }

}
