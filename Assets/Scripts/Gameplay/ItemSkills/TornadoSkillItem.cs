using UnityEngine;
using System.Collections;
using BeardedManStudios.Forge.Networking.Unity;

public class TornadoSkillItem : SkillItem
{
    public override void UseSkill(Transform playerTransform)
    {
        Vector3 startSpawnPosition = playerTransform.position;
        Vector3 playerForward = playerTransform.forward.normalized;
        Vector3 spellOffset = playerForward;
        //startSpawnPosition += spellOffset;
        NetworkManager.Instance.InstantiateSkills(index: 4, position: startSpawnPosition, rotation: Quaternion.LookRotation(playerForward));
    }

}
