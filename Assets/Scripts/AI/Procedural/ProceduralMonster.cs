using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
class ProceduralMonster : MonoBehaviour
{
    [SerializeField] 
    public bool ChangeType = false;
    [SerializeField]
    private int type = 0;

    private void Awake()
    {
        if (ProceduralMonsterSpawner.HasInstance)
        {
            ProceduralMonsterSpawner.Instance.AddSpawnedMonster(this);
        }
    }

    public void OnValidate()
    {
        if (ChangeType && ProceduralMonsterSpawner.HasInstance)
        {
            ProceduralMonsterSpawner.Instance.SpawnMonsterAtLocation(transform.position, type);
            Destroy(gameObject);
        }

        ChangeType = false;
    }
 }