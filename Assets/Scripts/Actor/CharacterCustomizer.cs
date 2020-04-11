using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizer : MonoBehaviour
{
    [SerializeField]
    private Transform m_SkeletalRoot;

    [SerializeField]
    private GameObject m_CurrentHair, m_CurrentAccessory, m_CurrentCostume, m_CurrentFace, m_CurrentEyes;
    private Material m_CostumeColor, m_EyeColor, m_HairColor;

    [SerializeField]
    private bool m_DemoHairPhysics = false;

    private int m_AccessoryIndex = 0;
    private int m_CostumeIndex = 0;
    private int m_CostumeColorIndex = 0;
    private int m_FaceIndex = 0;
    private int m_HairIndex = 0;
    private int m_EyeColorIndex = 0;

    private GameObject[] m_AccessoryLibrary;
    private GameObject[] m_CostumeLibrary;
    private GameObject[] m_FaceLibrary;
    private GameObject[] m_HairLibrary;
    private GameObject[] m_EyeLibrary;

    private Material[] m_EyeColorLibrary;
    private Material[] m_HairColorLibrary;
    private Material[] m_CostumeColorLibrary;

    private bool m_RequiresUpdate = true;
    private Dictionary<string, Transform> m_SkeletonCache = new Dictionary<string, Transform>();
    private Transform[] m_Bones;

    private void Awake()
    {
        CacheSkeletalTransform();
        SetupLastKnownSelections();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_AccessoryLibrary = Resources.LoadAll<GameObject>("CharacterCustomizer/Accessory");
        m_CostumeLibrary = Resources.LoadAll<GameObject>("CharacterCustomizer/Costume");
        m_FaceLibrary = Resources.LoadAll<GameObject>("CharacterCustomizer/Face");
        m_HairLibrary = Resources.LoadAll<GameObject>("CharacterCustomizer/Hair");
        m_EyeLibrary = Resources.LoadAll<GameObject>("CharacterCustomizer/Eye");

        m_CostumeColorLibrary = Resources.LoadAll<Material>("CharacterCustomizer/Costume/Material");
        m_HairColorLibrary = Resources.LoadAll<Material>("CharacterCustomizer/Hair/Material");
        m_EyeColorLibrary = Resources.LoadAll<Material>("CharacterCustomizer/Eye/Material");

        m_AccessoryIndex = m_AccessoryIndex >= m_AccessoryLibrary.Length ? 0 : m_AccessoryIndex;
        m_FaceIndex = m_FaceIndex >= m_FaceLibrary.Length ? 0 : m_FaceIndex;
        m_CostumeIndex = m_CostumeIndex >= m_CostumeLibrary.Length ? 0 : m_CostumeIndex;
        m_CostumeColorIndex = m_CostumeColorIndex >= m_CostumeColorLibrary.Length ? 0 : m_CostumeColorIndex;
        m_EyeColorIndex = m_EyeColorIndex >= m_EyeColorLibrary.Length ? 0 : m_EyeColorIndex;

        RefreshAll();
    }

    private void CacheSkeletalTransform()
    {
        m_Bones = m_SkeletalRoot.transform.GetComponentsInChildren<Transform>();

        foreach(Transform transform in m_Bones)
        {
            m_SkeletonCache.Add(transform.name, transform);
        }
    }

    private void AssembleBones(Transform[] target, SkinnedMeshRenderer mesh)
    {
        for (int i = 0; i < mesh.bones.Length; ++i)
        {
            try
            {
                Transform correctBone = m_SkeletonCache[mesh.bones[i].name];
                target[i] = correctBone;
            }
            catch
            {
                Debug.LogError("Cannot find a corresponding bone in character skeleton! Check the data " + mesh.name + " for compatibility!");
                Debug.LogError("Unknown Bone Name: " + mesh.bones[i].name);
            }
        }
    }

    /**
     * Hair is handled differently since it is difficult to get hair skeleton to
     * be compatible from model to model. Instead, we use spring bone to simulate 
     * hair and instantiate the hair's unique skeleton completely. This ironically
     * makes hair the easiest to swap.
     */
    public void SetHair(int index)
    {
        GameObject newHair = Instantiate(m_HairLibrary[index % m_HairLibrary.Length]);
        newHair.transform.parent = m_CurrentHair.transform.parent;
        newHair.transform.position = m_CurrentHair.transform.position;
        newHair.transform.localRotation = m_CurrentHair.transform.localRotation;
        newHair.transform.localScale = m_CurrentHair.transform.localScale;

        // Exaggerate hair physics for demo purposes. This should only be enabled
        // when the character's range of movement is restricted (for eg. in the lobby)
        if (m_DemoHairPhysics)
            newHair.GetComponent<UnityChan.SpringManager>().dynamicRatio = 1;

        Destroy(m_CurrentHair);
        m_CurrentHair = newHair;
        m_HairIndex = index;
    }

    public void SetAccessory(int index)
    {
        GameObject newAccessory = Set(index, m_AccessoryLibrary, m_CurrentAccessory);
        m_CurrentAccessory = newAccessory;
        m_AccessoryIndex = index;
    }

    public void SetCostume(int index)
    {
        GameObject newObj = Set(index, m_CostumeLibrary, m_CurrentCostume);
        m_CurrentCostume = newObj;
        m_CostumeIndex = index;
        SetCostumeColor(m_CostumeColorIndex);
    }

    public void SetFace(int index)
    {
        GameObject newObj = Set(index, m_FaceLibrary, m_CurrentFace);
        m_CurrentFace = newObj;
        m_FaceIndex = index;
    }

    // Pack data into byte sized blocks
    public ulong GetDataPacked()
    {
        ulong packed = 0;
        packed |= (ulong)(byte)m_AccessoryIndex << 0;
        packed |= (ulong)(byte)m_CostumeColorIndex << 8;
        packed |= (ulong)(byte)m_CostumeIndex << 16;
        packed |= (ulong)(byte)m_EyeColorIndex << 24;
        packed |= (ulong)(byte)m_FaceIndex << 32;
        packed |= (ulong)(byte)m_HairIndex << 40;

        return packed;
    }

    public void SetDataPacked(ulong packedData)
    {
        m_AccessoryIndex = (byte)(packedData >> 0);
        m_CostumeColorIndex = (byte)(packedData >> 8);
        m_CostumeIndex = (byte)(packedData >> 16);
        m_EyeColorIndex = (byte)(packedData >> 24);
        m_FaceIndex = (byte)(packedData >> 32);
        m_HairIndex = (byte)(packedData >> 40);
        RefreshAll();
    }

    protected GameObject Set(int index, GameObject[] assetLibrary, GameObject currentObj)
    {
        if (index < 0)
            return null;

        if (assetLibrary.Length <= 0)
            return null;

        if (index >= assetLibrary.Length)
            index = index % assetLibrary.Length;

        GameObject newObj = new GameObject(assetLibrary[index].name);

        if (assetLibrary[index] == null)
            return newObj;

        newObj.transform.parent = currentObj.transform.parent;
        newObj.transform.localPosition = currentObj.transform.localPosition;
        newObj.transform.localRotation = currentObj.transform.localRotation;
        newObj.transform.localScale = currentObj.transform.localScale;

        SkinnedMeshRenderer prefabRenderer = assetLibrary[index].GetComponentInChildren<SkinnedMeshRenderer>();

        if (prefabRenderer != null)
        {
            SkinnedMeshRenderer newRenderer = newObj.AddComponent(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;

            Transform[] newBones = new Transform[prefabRenderer.bones.Length];
            AssembleBones(newBones, prefabRenderer);

            // Update reference
            newRenderer.bones = newBones;
            newRenderer.sharedMesh = prefabRenderer.sharedMesh;
            newRenderer.sharedMaterials = prefabRenderer.sharedMaterials;
        }

        Destroy(currentObj);
        return newObj;
    }

    private void SetupLastKnownSelections()
    {
        Debug.Log("Character Customization Settings Loaded");
        m_HairIndex             = PlayerPrefs.GetInt("CharacterCustomization.HairType", 0);
        m_FaceIndex             = PlayerPrefs.GetInt("CharacterCustomization.FaceType", 0);
        m_CostumeIndex          = PlayerPrefs.GetInt("CharacterCustomization.CostumeType", 0);
        m_AccessoryIndex        = PlayerPrefs.GetInt("CharacterCustomization.AccessoryType", 0);
        m_CostumeColorIndex     = PlayerPrefs.GetInt("CharacterCustomization.Costume.Color", 0);
        m_EyeColorIndex         = PlayerPrefs.GetInt("CharacterCustomization.Eye.Color", 0);
    }

    public void Randomize()
    {
        m_AccessoryIndex = Random.Range(0, m_AccessoryLibrary.Length);
        m_HairIndex = Random.Range(0, m_HairLibrary.Length);
        m_FaceIndex = Random.Range(0, m_FaceLibrary.Length);
        m_CostumeIndex = Random.Range(0, m_CostumeLibrary.Length);

        m_CostumeColorIndex = Random.Range(0, m_CostumeColorLibrary.Length);
        m_EyeColorIndex = Random.Range(0, m_EyeColorLibrary.Length);

        RefreshAll();
    }

    public void Save()
    {
        Debug.Log("Character Customization Saved");
        PlayerPrefs.SetInt("CharacterCustomization.HairType", m_HairIndex);
        PlayerPrefs.SetInt("CharacterCustomization.FaceType", m_FaceIndex);
        PlayerPrefs.SetInt("CharacterCustomization.CostumeType", m_CostumeIndex);
        PlayerPrefs.SetInt("CharacterCustomization.AccessoryType", m_AccessoryIndex);
        PlayerPrefs.SetInt("CharacterCustomization.Eye.Color", m_EyeColorIndex);
        PlayerPrefs.SetInt("CharacterCustomization.Costume.Color", m_CostumeColorIndex);
    }

    public void SetHairColor(int index)
    {
    }

    public void SetEyeColor(int index)
    {
        SetMaterial(index, m_EyeColorLibrary, m_CurrentEyes);
        m_EyeColorIndex = index;
    }

    public void SetCostumeColor(int index)
    {
        SetMaterial(index, m_CostumeColorLibrary, m_CurrentCostume);
        m_CostumeColorIndex = index;
    }

    private void SetMaterial(int index, Material[] assetLibrary, GameObject currentObj)
    {
        if (index < 0)
            return;

        if (assetLibrary.Length <= 0)
            return;

        if (index >= assetLibrary.Length)
            index = index % assetLibrary.Length;

        currentObj.GetComponent<SkinnedMeshRenderer>().material = assetLibrary[index];
    }

    private void RefreshAll()
    {
        SetHair(m_HairIndex);
        SetCostume(m_CostumeIndex);
        SetFace(m_FaceIndex);
        SetAccessory(m_AccessoryIndex);
        SetEyeColor(m_EyeColorIndex);
        SetCostumeColor(m_CostumeColorIndex);
    }

}
