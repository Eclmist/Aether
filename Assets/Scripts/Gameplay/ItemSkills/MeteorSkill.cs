using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;

public class MeteorSkill : ItemSkill
{
    [SerializeField]
    private LayerMask m_LayerMask = new LayerMask();

    [SerializeField]
    private GameObject m_MeteorPrefab;

    private const int m_ICON_INDEX = 1;

    private const int m_MAX_MOVES = 1;

    public override void InitializeSkill()
    {
        SetUpSkill(m_MAX_MOVES, m_ICON_INDEX);
    }
    public override void UseSkill()
    {
        //Vector3 forward = Camera.main.transform.forward;
        //forward.y = 0.0f;
        //Ray ray = new Ray(Camera.main.transform.position + new Vector3(0, 0, 0), Camera.main.transform.forward);

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, m_LayerMask))
        {
            NetworkManager.Instance.InstantiateSkills(index: 0, position: hit.point, rotation: Quaternion.identity);

            ///////////////////////////////////////////////////////////
            // Code below is for instantiation of game prefabs locally
            //////////////////////////////////////////////////////////
            

            //Debug.Log(hit.transform.position);
            //GameObject meteor = Instantiate(m_MeteorPrefab, hit.point, Quaternion.identity);
            //Destroy(meteor, 7.0f);
        }
    }

}
