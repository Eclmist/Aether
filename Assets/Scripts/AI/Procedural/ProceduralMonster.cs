using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ProceduralMonster : MonoBehaviour
{
    [SerializeField] 
    public bool ChangeType = false;
    [SerializeField]
    private int type = 0;

    private void Start()
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
            ProceduralMonsterSpawner.Instance.RemoveSpawnedMonster(this);
            EditorApplication.delayCall += () => { DestroyImmediate(gameObject); };
        }
        else if (ChangeType)
        {
            Debug.Log("Requires ProceduralMonsterSpawner " +
                      "Manager to be added to the scene for it to work. Prefab can be found in AI folder");
        }

        ChangeType = false;
    }
}