using UnityEngine;
using System.Collections;
using BeardedManStudios.Forge.Networking.Unity;

public class ShurikenSkill : SkillItem
{
    public override void UseSkill(Transform playerTransform)
    {
        Vector3 startSpawnPosition = playerTransform.position + new Vector3(0.0f, 1.2f, 0.0f);
        Vector3 playerForward = playerTransform.forward.normalized;
        Vector3 spellOffset = playerForward * 1.5f;
        startSpawnPosition += spellOffset;
        NetworkManager.Instance.InstantiateSkills(index: 6, position: startSpawnPosition, rotation: Quaternion.LookRotation(playerForward));
    }

}
