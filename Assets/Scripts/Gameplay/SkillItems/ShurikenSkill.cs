using UnityEngine;
using System.Collections;
using BeardedManStudios.Forge.Networking.Unity;

public class ShurikenSkill : SkillItem
{
    [SerializeField]
    private LayerMask m_LayerMask;
    public override void UseSkill(Transform playerTransform)
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LayerMask))
        {
            Vector3 startSpawnPosition = playerTransform.position 
                + 25.0f * (playerTransform.forward.normalized)
                + new Vector3(0.0f, 1.6f, 0.0f);
            Vector3 hitPoint = hit.point - playerTransform.position;
            NetworkManager.Instance?.InstantiateSkills(index: 6, position: startSpawnPosition, rotation: Quaternion.LookRotation(hitPoint, playerTransform.up));
        }
        
    }

}
