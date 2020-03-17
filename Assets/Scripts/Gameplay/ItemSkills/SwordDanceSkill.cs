using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDanceSkill : ItemSkill
{
    [SerializeField]
    private LayerMask m_LayerMask = new LayerMask();

    [SerializeField]
    private GameObject m_SwordDanceStart;

    [SerializeField]
    private GameObject m_SwordDanceEnd;

    [SerializeField]
    private Transform m_CharacterTransform;

    private const int m_ICON_INDEX = 1;

    private const int m_MAX_MOVES = 1;

    public override void InitializeSkill()
    {
        SetUpSkill(m_MAX_MOVES, m_ICON_INDEX);
    }
    public override void UseSkill()
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0.0f;
        Ray ray = new Ray(Camera.main.transform.position + new Vector3(0, 0, 0), Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, m_LayerMask))
        {
            Debug.Log(hit.transform.position);
            GameObject swordDanceStart = Instantiate(m_SwordDanceStart, m_CharacterTransform.position, Quaternion.identity);
            GameObject swordDanceEnd = Instantiate(m_SwordDanceEnd, hit.point, Quaternion.identity);
            Destroy(swordDanceStart, 4.0f);
            Destroy(swordDanceEnd, 6.0f);
        }
    }

    
}
