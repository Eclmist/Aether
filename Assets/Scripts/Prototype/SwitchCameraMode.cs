using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aether.Prototype
{
    public class SwitchCameraMode : MonoBehaviour
    {
        public KeyCode m_TargetKey;

        [Header("Third Person Script References")]
        public Cinemachine.CinemachineBrain m_CinemachineBrainScript;
        public MeshRenderer m_PlayerMeshRenderer;

        [Header("First Person Script References")]
        public MouseLook m_MouseLookScript;
        public Camera m_MainCamera;
        public GameObject m_CrossHair;

        private bool m_IsFirstPerson = false;

        // Start is called before the first frame update
        void Start()
        {
            m_IsFirstPerson = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(m_TargetKey))
                SwitchMode();
        }

        void SwitchMode()
        {
            m_IsFirstPerson = !m_IsFirstPerson;

            m_CinemachineBrainScript.enabled = !m_IsFirstPerson;
            m_PlayerMeshRenderer.enabled = !m_IsFirstPerson;
            m_MouseLookScript.enabled = m_IsFirstPerson;
            m_MainCamera.fieldOfView = m_IsFirstPerson ? 100 : m_MainCamera.fieldOfView;
            m_MainCamera.transform.localPosition = m_IsFirstPerson ? new Vector3(0, 0.5f, 0) : m_MainCamera.transform.localPosition;
            m_CrossHair.SetActive(m_IsFirstPerson);
        }
    }
}
