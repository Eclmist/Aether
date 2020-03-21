using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;


public class SpellSkill : ItemSkill
{
    //[SerializeField]
    //private LayerMask m_LayerMask = new LayerMask();

    //[SerializeField]
    //private GameObject m_SpellPrefab;

    //[SerializeField]
    //private Transform m_PlayerTransform;

    // private Vector3 m_Spellcast_Offset = new Vector3(0.1f, 1.2f, 0.5f);

    private const int m_ICON_INDEX = 1;

    private const int m_MAX_MOVES = 1;

    private const bool m_IS_GROUND_SPELL = true;

    public override void InitializeSkill()
    {
        SetUpSkill(m_MAX_MOVES, m_ICON_INDEX, m_IS_GROUND_SPELL, ItemSkill.Skill.SKILL_LASER);
    }
    public override void UseSkill()
    {
        ///////////////////////////////////////////////////////////
        // Code below is for instantiation of game prefabs locally
        //////////////////////////////////////////////////////////

        //Vector3 forwardDirection = Camera.main.transform.forward;
        //forwardDirection.y = 0;
        //GameObject spell = Instantiate(m_SpellPrefab, m_PlayerTransform.position + new Vector3(0, 1.2f, 0), Quaternion.LookRotation(forwardDirection.normalized));
        //Destroy(spell, 6.0f);

        //Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit))
        //{
        Vector3 startSpawnPosition = PlayerManager.Instance.GetLocalPlayer().transform.position;
        Vector3 playerForward = PlayerManager.Instance.GetLocalPlayer().transform.forward.normalized;
        Vector3 spellOffset = playerForward * 2 + new Vector3(0f, 1.2f, 0f);
        startSpawnPosition += spellOffset;
        //Vector3 startSpawnDirection = hit.point - startSpawnPosition;

        NetworkManager.Instance.InstantiateSkills(index: 3, position: startSpawnPosition, rotation: Quaternion.LookRotation(playerForward));
        //}

    }
}
