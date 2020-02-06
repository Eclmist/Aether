using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateNavigation : MonoBehaviour
{
    private Transform m_NavigationTransform;
    // Start is called before the first frame update
    void Start()
    {
        m_NavigationTransform = this.gameObject.transform;   
    }

    // Update is called once per frame
    void Update()
    {
        m_NavigationTransform.Rotate(new Vector3(0f, 0f, -1f));
    }
}
