using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProceduralMonsterSpawner : Singleton<ProceduralMonsterSpawner>
{
    [System.Serializable]
    public class MonsterMat
    {
        public string m_matColor;
        public Material[] m_materials;
    }
    

    [SerializeField] private GameObject[] m_monster_prefabs;
    [SerializeField] private int m_monsterIndex;
    [SerializeField] private MonsterMat[] m_mushroomMonsterMaterials;
    [SerializeField] private MonsterMat[] m_plantMonsterMaterials;
    [SerializeField] private MonsterMat[] m_rockMonsterMaterials;
    [SerializeField] private MonsterMat[] m_wormMonsterMaterials;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_mutationChance = 0.3f;

    private LinkedList<ProceduralMonster> m_MonstersSpawned = new LinkedList<ProceduralMonster>();
    [SerializeField] 
    private bool m_ChangeAllMonsters = false;

    private void Awake()
    {
    }

    public void AddSpawnedMonster(ProceduralMonster monster)
    {
        m_MonstersSpawned.AddFirst(monster);
    }

    public void RemoveSpawnedMonster(ProceduralMonster monster)
    {
        m_MonstersSpawned.Remove(monster);
    }

    void OnValidate()
    {
        if (m_ChangeAllMonsters)
        {
            foreach (ProceduralMonster monster in m_MonstersSpawned)
            {
                monster.ChangeType = true;
            }
        }

        m_ChangeAllMonsters = false;
    }

    public void SpawnMonsterAtLocation(Vector3 destination, int monsterIndex)
    {
        if (CheckReferences())
        {
            return;
        }
        var monsterPrefab = m_monster_prefabs[monsterIndex];
        var monster = Instantiate(monsterPrefab);
        monster.transform.parent = gameObject.transform;
        var mats = GetMaterials(monsterPrefab);

        monster.transform.position = destination;
        monster.transform.localScale *= 1.5f - Random.value; //randomizes scale based on uniform distribution
        bool mutation = Random.value < m_mutationChance;
        Vector3 color = new Vector3(1f, 1f, 1f) - 0.5f * new Vector3(Random.value, Random.value, Random.value);
        Color randomMutateColor = new Color(color.x, color.y, color.z); //lighter colours

        foreach (var skinnedMeshRenderer in monster.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            skinnedMeshRenderer.material = Instantiate<Material>(FindMaterialOrRandom(mats, skinnedMeshRenderer.gameObject.name));
            if(mutation)
                skinnedMeshRenderer.material.color = randomMutateColor;

            for (var i = 0; i < skinnedMeshRenderer.sharedMesh.blendShapeCount; i++)
            {
                int weight = Random.Range(0, 100); //Weights are between 0 to 100

                skinnedMeshRenderer.SetBlendShapeWeight(i, weight);
            }
        }
    }

    private bool CheckReferences()
    {
        bool CheckArray(object[] arr)
        {
            return arr != null || arr.Length != 0;
        }

        if (CheckArray(m_monster_prefabs) || CheckArray(m_mushroomMonsterMaterials) ||
            CheckArray(m_plantMonsterMaterials)
            || CheckArray(m_rockMonsterMaterials) || CheckArray(m_wormMonsterMaterials))
        {
            return false;
        }
        Debug.LogError("Empty references in Procedural Monster Spawner");
        return true;
    }

    private Material FindMaterialOrRandom(Material[] materials, string name)
    {
        foreach (var mat in materials)
        {
            if (mat.name.Contains(name))
                return mat;
        }

        return materials[Random.Range(0, materials.Length)];
    } 
    
    private Material[] GetMaterials(GameObject monsterPrefab)
    {
        var strings = monsterPrefab.ToString().Split('_');
        string name = strings[1]; // + " " + strings[2];
        MonsterMat[] mats;
        // Material[] mats = Resources.LoadAll("SFBayStudios/SFB " + name + "/Exported Textures & Materials", 
        //    typeof(Material)).Cast<Material>().ToArray();
        switch (name)
        {
            case "Mushroom":
                mats = m_mushroomMonsterMaterials;
                break;
            case "Plant":
                mats = m_plantMonsterMaterials;
                break;
            case "Rock":
                mats = m_rockMonsterMaterials;
                break;
            case "Worm":
                mats = m_wormMonsterMaterials;
                break;
            default:
                mats = null;
                Debug.LogError("Wrong monster prefab in GetMaterials in ProceduralMonsterSpawner");
                break;
        }

        return mats[Random.Range(0, mats.Length)].m_materials;
    }

    /*
     * Debug editor
     */
    /*public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray))
                SpawnMonsterAtLocation(transform.position, m_monsterIndex);
        }
    }*/
}