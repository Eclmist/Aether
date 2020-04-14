using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;

public class GroundSpikeSkillItem : SkillItem
{
    public override void UseSkill(Transform playerTransform)
    {
        Vector3 startSpawnPosition = playerTransform.position;
        Vector3 playerForward = playerTransform.forward.normalized * 2f;
        Vector3 spellOffset = playerForward;
        startSpawnPosition += spellOffset;
        NetworkManager.Instance.InstantiateSkills(index: 5, position: startSpawnPosition, rotation: Quaternion.LookRotation(playerForward));
    }
}
