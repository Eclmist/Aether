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

    private Vector3 m_Spellcast_Offset = new Vector3(0f, 1.2f, 0f);

    private const int m_ICON_INDEX = 1;

    private const int m_MAX_MOVES = 1;


    public override void InitializeSkill()
    {
        SetUpSkill(m_MAX_MOVES, m_ICON_INDEX);
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

       
        Vector3 startSpawnPosition = PlayerManager.Instance.GetLocalPlayer().transform.position;
        startSpawnPosition += m_Spellcast_Offset;
        Vector3 startSpawnDirection = PlayerManager.Instance.GetLocalPlayer().transform.forward;
        NetworkManager.Instance.InstantiateSkills(index: 3, position: startSpawnPosition, rotation: Quaternion.LookRotation(startSpawnDirection));

    }
}
