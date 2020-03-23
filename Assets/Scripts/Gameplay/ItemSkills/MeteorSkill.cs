using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;

public class MeteorSkill : ItemSkill
{
    [SerializeField]
    private LayerMask m_LayerMask = new LayerMask();
    public override void UseSkill(Transform playerTransform)
    {
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
