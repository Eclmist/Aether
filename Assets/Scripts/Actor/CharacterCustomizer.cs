using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizer : MonoBehaviour
{
    public enum Type
    {
        TYPE_ACCESSORY,
        TYPE_COSTUME,
        TYPE_FACE,
        TYPE_HAIR,
        TYPE_EYE
    }

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
        Destroy(m_CurrentHair);
        m_CurrentHair = newHair;
        m_CurrentHairIndex = index;
    }

    public void SetAccessory(int index)
    {
        if (index == m_CurrentAccessoryIndex)
            return;

        GameObject newAccessory = Set(Type.TYPE_ACCESSORY, index, m_AccessoryLibrary, m_CurrentAccessory);
        m_CurrentAccessory = newAccessory;
        m_CurrentAccessoryIndex = index;
    }

    public void SetCostume(int index)
    {
        if (index == m_CurrentCostumeIndex)
            return;

        GameObject newObj = Set(Type.TYPE_COSTUME, index, m_CostumeLibrary, m_CurrentCostume);
        m_CurrentCostume = newObj;
        m_CurrentCostumeIndex = index;
    }
    public void SetEyes(int index)
    {
        if (index == m_CurrentEyeIndex)
            return;

        GameObject newObj = Set(Type.TYPE_EYE, index, m_EyeLibrary, m_CurrentEyes);
        m_CurrentEyes = newObj;
        m_CurrentEyeIndex = index;
    }
    public void SetFace(int index)
    {
        if (index == m_CurrentFaceIndex)
            return;

        GameObject newObj = Set(Type.TYPE_FACE, index, m_FaceLibrary, m_CurrentFace);
        m_CurrentFace = newObj;
        m_CurrentFaceIndex = index;
    }

    protected GameObject Set(Type type, int index, GameObject[] assetLibrary, GameObject currentObj)
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
}
