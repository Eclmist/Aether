using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;

public class TornadoSkill : ItemSkill
{

    //[SerializeField]
    //private GameObject m_SpellPrefab;

    //[SerializeField]
    //private Transform m_PlayerTransform;

    private const int m_ICON_INDEX = 1;

    private const int m_MAX_MOVES = 1;

    private const bool m_IS_GROUND_SPELL = true;

    public override void InitializeSkill()
    {
        SetUpSkill(m_MAX_MOVES, m_ICON_INDEX, m_IS_GROUND_SPELL, ItemSkill.Skill.SKILL_TORNADO);
    }
    public override void UseSkill()
    {
        Vector3 startSpawnPosition = PlayerManager.Instance.GetLocalPlayer().transform.position;

        NetworkManager.Instance.InstantiateSkills(index: 4, position: startSpawnPosition, rotation: Quaternion.identity);
    }
}
