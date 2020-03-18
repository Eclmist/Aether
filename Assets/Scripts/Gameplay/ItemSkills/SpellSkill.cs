using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSkill : ItemSkill
{
    [SerializeField]
    private LayerMask m_LayerMask = new LayerMask();

    [SerializeField]
    private GameObject m_SpellPrefab;

    [SerializeField]
    private Transform m_PlayerTransform;

    private const int m_ICON_INDEX = 1;

    private const int m_MAX_MOVES = 1;


    public override void InitializeSkill()
    {
        SetUpSkill(m_MAX_MOVES, m_ICON_INDEX);
        // m_PlayerTransform = PlayerManager.Instance.GetLocalPlayer().transform;
    }
    public override void UseSkill()
    {
        Vector3 forwardDirection = Camera.main.transform.forward;
        forwardDirection.y = 0;
        GameObject spell = Instantiate(m_SpellPrefab, m_PlayerTransform.position + new Vector3(0, 1.2f, 0), Quaternion.LookRotation(forwardDirection.normalized));
        Debug.Log(m_PlayerTransform.position);
        Debug.Log(spell.transform.position);
        Destroy(spell, 6.0f);
    }

    
}
