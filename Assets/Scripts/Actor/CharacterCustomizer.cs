using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizer : MonoBehaviour
{
    [SerializeField]
    private Transform m_SkeletalRoot;

    [SerializeField]
    private int m_AccessoryIndex = 0;
    private int m_CurrentAccessoryIndex = -1;
    [SerializeField]
    private int m_CostumeIndex = 0;
    private int m_CurrentCostumeIndex = -1;
    [SerializeField]
    private int m_FaceIndex = 0;
    private int m_CurrentFaceIndex = -1;
    [SerializeField]
    private int m_HairIndex = 0;
    private int m_CurrentHairIndex = -1;
    [SerializeField]
    private int m_EyeIndex = 0;
    private int m_CurrentEyeIndex = -1;

    [SerializeField]
    private GameObject m_CurrentHair, m_CurrentAccessory, m_CurrentCostume, m_CurrentFace, m_CurrentEyes;

    [SerializeField]
    private bool m_DemoHairPhysics = false;

    private Avatar m_Avatar;

    private GameObject[] m_AccessoryLibrary;
    private GameObject[] m_CostumeLibrary;
    private GameObject[] m_FaceLibrary;
    private GameObject[] m_HairLibrary;
    private GameObject[] m_EyeLibrary;

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

        m_AccessoryIndex = m_AccessoryIndex >= m_AccessoryLibrary.Length ? 0 : m_AccessoryIndex;
        m_CostumeIndex = m_CostumeIndex >= m_CostumeLibrary.Length ? 0 : m_CostumeIndex;
        m_FaceIndex = m_FaceIndex >= m_FaceLibrary.Length ? 0 : m_FaceIndex;
        m_HairIndex = m_HairIndex >= m_HairLibrary.Length ? 0 : m_HairIndex;
        m_EyeIndex = m_EyeIndex >= m_EyeLibrary.Length ? 0 : m_EyeIndex;

        SetHair(m_HairIndex);
        SetEyes(m_EyeIndex);
        SetFace(m_FaceIndex);
        SetCostume(m_CostumeIndex);
        SetAccessory(m_AccessoryIndex);
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
        if (index == m_CurrentHairIndex)
            return;

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
        m_CurrentHairIndex = index;
    }

    public void SetAccessory(int index)
    {
        if (index == m_CurrentAccessoryIndex)
            return;

        GameObject newAccessory = Set(index, m_AccessoryLibrary, m_CurrentAccessory);
        m_CurrentAccessory = newAccessory;
        m_CurrentAccessoryIndex = index;
    }

    public void SetCostume(int index)
    {
        if (index == m_CurrentCostumeIndex)
            return;

        GameObject newObj = Set(index, m_CostumeLibrary, m_CurrentCostume);
        m_CurrentCostume = newObj;
        m_CurrentCostumeIndex = index;
    }
    public void SetEyes(int index)
    {
        if (index == m_CurrentEyeIndex)
            return;

        GameObject newObj = Set(index, m_EyeLibrary, m_CurrentEyes);
        m_CurrentEyes = newObj;
        m_CurrentEyeIndex = index;
    }
    public void SetFace(int index)
    {
        if (index == m_CurrentFaceIndex)
            return;

        GameObject newObj = Set(index, m_FaceLibrary, m_CurrentFace);
        m_CurrentFace = newObj;
        m_CurrentFaceIndex = index;
    }

    protected GameObject Set(int index, GameObject[] assetLibrary, GameObject currentObj)
    {
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
        m_HairIndex      = PlayerPrefs.GetInt("CharacterCustomization.HairType", 0);
        m_EyeIndex       = PlayerPrefs.GetInt("CharacterCustomization.EyeType", 0);
        m_FaceIndex      = PlayerPrefs.GetInt("CharacterCustomization.FaceType", 0);
        m_CostumeIndex   = PlayerPrefs.GetInt("CharacterCustomization.CostumeType", 0);
        m_AccessoryIndex = PlayerPrefs.GetInt("CharacterCustomization.AccessoryType", 0);
    }

    public void Randomize()
    {
        m_AccessoryIndex = Random.Range(0, m_AccessoryLibrary.Length);
        m_HairIndex = Random.Range(0, m_HairLibrary.Length);
        m_EyeIndex = Random.Range(0, m_EyeLibrary.Length);
        m_FaceIndex = Random.Range(0, m_FaceLibrary.Length);
        m_CostumeIndex = Random.Range(0, m_CostumeLibrary.Length);

        SetHair(m_HairIndex);
        SetEyes(m_EyeIndex);
        SetFace(m_FaceIndex);
        SetCostume(m_CostumeIndex);
        SetAccessory(m_AccessoryIndex);
    }

    public void Save()
    {
        Debug.Log("Character Customization Saved");
        PlayerPrefs.SetInt("CharacterCustomization.HairType", m_CurrentHairIndex);
        PlayerPrefs.SetInt("CharacterCustomization.EyeType", m_CurrentEyeIndex);
        PlayerPrefs.SetInt("CharacterCustomization.FaceType", m_CurrentFaceIndex);
        PlayerPrefs.SetInt("CharacterCustomization.CostumeType", m_CurrentCostumeIndex);
        PlayerPrefs.SetInt("CharacterCustomization.AccessoryType", m_CurrentAccessoryIndex);
    }
}
