using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHighlight : MonoBehaviour
{
    private Renderer[] m_Renderers;
    private bool ran;
    void Start()
    {
        GetRenderers();
        ran = false;

    }

    private void Update()
    {
        if (!ran)
        {
            CheckRenderers();
            ran = true;
        }
        

    }

    void GetRenderers()
    {
        m_Renderers = this.gameObject.GetComponentsInChildren<Renderer>();
    }

    void CheckRenderers()
    {
        List<Material> materials = new List<Material>();
        List<Material> sharedMaterials = new List<Material>();
        foreach (Renderer r in m_Renderers)
        {
            if (r != null)
            {
                r.GetMaterials(materials);
                r.GetSharedMaterials(sharedMaterials);
                foreach (Material m in materials)
                {
                    if (m != null)
                    {
                        Debug.Log(m.name);
                        m.SetColor("_OutlineColor", Color.red);
                        r.material = m;
                    }
                }
                
            }
        }
    }

    //private void SetMaterialOpacity()
    //{
        //SkinnedMeshRenderer[] childrenRenderers = m_Player.GetComponentsInChildren<SkinnedMeshRenderer>();
        //Debug.Log(childrenRenderers.Length);
        //foreach (SkinnedMeshRenderer renderer in childrenRenderers)
        //{
        //    Debug.Log(renderer.material.color.a);
        //    renderer.material.SetFloat("_OutlineOpacity", 0);
        //}
    //}
}
