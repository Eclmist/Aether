using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;

public class GroundSpikeSkillItem : SkillItem
{
    public override void UseSkill(Transform playerTransform)
    {
        GroundSpikeSkill spikeSkill = NetworkManager.Instance.InstantiateSkills(index: 5, position: playerTransform.position, rotation: Quaternion.LookRotation(playerTransform.forward.normalized)) as GroundSpikeSkill;
        
        // Activate game object in order to call gameobject's coroutine
        spikeSkill.gameObject.SetActive(true);
        spikeSkill.SpawnSpikeSpell(playerTransform);
    }
}
