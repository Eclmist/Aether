using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralMonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] m_monster_prefabs;
    [SerializeField] private int m_monsterIndex;

    public void SpawnMonsterAtLocation(Vector3 destination, int monsterIndex)
    {
        GameObject monster = Instantiate(m_monster_prefabs[monsterIndex]);
        monster.transform.position = destination;
        
        //todo: color
        foreach (var skinnedMeshRenderer in monster.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            for (int i = 0; i < skinnedMeshRenderer.sharedMesh.blendShapeCount; i++)
            {
                int weight = Random.Range(0, 100);
                skinnedMeshRenderer.SetBlendShapeWeight(i, weight);
            }
            
        }
    }

    public void Init()
    {
        SpawnMonsterAtLocation(new Vector3(), m_monsterIndex);
    }

    /*
     * Debug editor
     */
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray))
                SpawnMonsterAtLocation(transform.position, m_monsterIndex);
        }
    }
}
