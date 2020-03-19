using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;
public class SwordDanceSkill : ItemSkill
{
    [SerializeField]
    private LayerMask m_LayerMask = new LayerMask();

    //[SerializeField]
    //private GameObject m_SwordDanceStart;

    //[SerializeField]
    //private GameObject m_SwordDanceEnd;

    //[SerializeField]
    //private Transform m_PlayerTransform;

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
            Vector3 startSpawnPosition = PlayerManager.Instance.GetLocalPlayer().transform.position;

            // Instantiate SwordDanceStart at the player's current position
            NetworkManager.Instance.InstantiateSkills(index: 1, position: startSpawnPosition, rotation: Quaternion.identity);

            // Instantiate SwordDanceEnd at the player's click point
            NetworkManager.Instance.InstantiateSkills(index: 2, position: hit.point, rotation: Quaternion.identity);

            ///////////////////////////////////////////////////////////
            // Code below is for instantiation of game prefabs locally
            //////////////////////////////////////////////////////////

            //GameObject swordDanceStart = Instantiate(m_SwordDanceStart, startSpawnPosition, Quaternion.identity);
            //GameObject swordDanceEnd = Instantiate(m_SwordDanceEnd, hit.point, Quaternion.identity);
            //Destroy(swordDanceStart, 4.0f);
            //Destroy(swordDanceEnd, 6.0f);
        }
    }

    
}
