using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlacement : MonoBehaviour
{
    public GameObject m_FollowTarget;
    public Vector3 m_FollowOffset;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
        Vector3 target = m_FollowTarget.transform.position;
        target += -Camera.main.transform.right * m_FollowOffset.x;
        target += m_FollowOffset.y * Vector3.up;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 8);

    }
}
