using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;

public class TornadoSkill : ItemSkill
{

    public override void UseSkill()
    {
        Vector3 startSpawnPosition = PlayerManager.Instance.GetLocalPlayer().transform.position;
        Vector3 playerForward = PlayerManager.Instance.GetLocalPlayer().transform.forward.normalized;
        Vector3 spellOffset = playerForward;
        startSpawnPosition += spellOffset;

        NetworkManager.Instance.InstantiateSkills(index: 4, position: startSpawnPosition, rotation: Quaternion.LookRotation(playerForward));

    }
}
