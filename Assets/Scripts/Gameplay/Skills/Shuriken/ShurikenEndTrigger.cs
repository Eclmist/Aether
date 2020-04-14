using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;

public class ShurikenEndTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject m_HitPrefab;

    private void OnTriggerEnter(Collider other)
    {
        NetworkManager.Instance.InstantiateSkills(index: 7, position: transform.position, rotation: Quaternion.identity);
        Destroy(gameObject);
    }
}
