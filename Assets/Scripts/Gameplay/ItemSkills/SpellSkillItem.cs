using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;

public class SpellSkillItem : SkillItem
{

    // Height offset of character for spell cast
    private Vector3 m_HeightOffset = new Vector3(0f, 1.2f, 0f);

    public override void UseSkill(Transform playerTransform)
    {
        ///////////////////////////////////////////////////////////
        // Code below is for instantiation of game prefabs locally
        //////////////////////////////////////////////////////////

        //Vector3 forwardDirection = Camera.main.transform.forward;
        //forwardDirection.y = 0;
        //GameObject spell = Instantiate(m_SpellPrefab, m_PlayerTransform.position + new Vector3(0, 1.2f, 0), Quaternion.LookRotation(forwardDirection.normalized));
        //Destroy(spell, 6.0f);

        Vector3 startSpawnPosition = playerTransform.position;
        Vector3 playerForward = playerTransform.forward.normalized;

        // Shift spell to instantiate in front of player and at player's chest area
        Vector3 spellOffset = playerForward * 2 + m_HeightOffset;
        startSpawnPosition += spellOffset;

        NetworkManager.Instance.InstantiateSkills(index: 3, position: startSpawnPosition, rotation: Quaternion.LookRotation(playerForward));
    }
}
