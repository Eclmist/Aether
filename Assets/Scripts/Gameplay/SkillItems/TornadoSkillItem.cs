using UnityEngine;
using System.Collections;
using BeardedManStudios.Forge.Networking.Unity;

public class TornadoSkillItem : SkillItem
{
    [SerializeField]
    private LayerMask m_LayerMask;
    public override void UseSkill(Transform playerTransform)
    {

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LayerMask))
        {
            Vector3 startSpawnPosition = playerTransform.position + 5.0f * (playerTransform.forward.normalized);
            Vector3 hitPoint = hit.point - playerTransform.position;
            //Vector3 spellOffset = playerForward * 1.5f;
            //startSpawnPosition += spellOffset;
            NetworkManager.Instance.InstantiateSkills(index: 4, position: startSpawnPosition, rotation: Quaternion.LookRotation(hitPoint, playerTransform.up));
        }

    }

}
