using System.Collections;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;

[RequireComponent(typeof(MonsterObjectNetworkObject))]
public class MonsterNetworkHandler : MonsterObjectBehavior
{
    
    private void Update()
    {
        if (networkObject == null)
        {
            Debug.LogWarning("No monster network object found");
            return;
        }

        if (networkObject.IsOwner)
        {
            // If we are the owner of the object we should send the new position
            // and rotation across the network for receivers to move to in the above code
            networkObject.position = transform.position;
            networkObject.rotation = transform.rotation;

        }
        else
        {
            transform.position = networkObject.position;
            transform.rotation = networkObject.rotation;
        }

    }

}
