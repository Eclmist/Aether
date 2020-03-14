using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchMeteor : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_LayerMask = new LayerMask();

    [SerializeField]
    private GameObject m_MeteorPrefab;

    private void Start()
    {
        // For test purpose
        AetherInput.GetPlayerActions().Fire.performed += HandleLaunchMeteor;
    }

    // Code below can be used to launch meteor into the scene, with respect to the reticle at centre of screen.
    void HandleLaunchMeteor(InputAction.CallbackContext ctx)
    {

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0.0f;
        Ray ray = new Ray(Camera.main.transform.position + new Vector3(0, 0, 0), Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, m_LayerMask))
        {
            //Debug.DrawRay(Camera.main.transform.position, new Vector2(Screen.width / 2, Screen.height / 2), Color.green);
            Debug.Log(hit.transform.position);
            GameObject meteor = Instantiate(m_MeteorPrefab, hit.point, Quaternion.identity);
            Destroy(meteor, 7.0f);
        }
    }
}
