using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;


public class SwordDanceSkillItem : SkillItem
{
    [SerializeField]
    private LayerMask m_LayerMask = new LayerMask();


    public override void UseSkill(Transform playerTransform)
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, m_LayerMask))
        {
            // Instantiate SwordDanceStart at the player's current position
            NetworkManager.Instance.InstantiateSkills(index: 1, position: playerTransform.position, rotation: Quaternion.identity);

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
