using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHighlight : MonoBehaviour
{
    private Renderer[] m_Renderers;
    // Start is called before the first frame update
    void Start()
    {
        m_Renderers = this.gameObject.GetComponentsInChildren<Renderer>();
    }


    private void Update()
    {        
        foreach (Renderer r in m_Renderers)
        {
            if (r == null)
            {
                continue;
            } 
            else
            {
                Material[] materials = r.GetComponents<Material>();
                if (materials != null)
                {
                    foreach (Material m in materials)
                    {
                        if (m != null)
                        {
                            m.SetFloat("_OutlineOpacity", 1);
                        }
                    }
                }
                else
                {
                    break;
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
