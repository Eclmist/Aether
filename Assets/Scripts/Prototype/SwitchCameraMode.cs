using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aether.Prototype
{
    public class SwitchCameraMode : MonoBehaviour
    {
        public KeyCode m_TargetKey;

        [Header("Third Person Script References")]
        public GameObject[] m_ThirdPersonTogglables;
        public MonoBehaviour[] m_ThirdPersonTogglableScripts;

        [Header("First Person Script References")]
        public Camera m_MainCamera;
        public GameObject[] m_FirstPersonTogglables;
        public MonoBehaviour[] m_FirstPersonTogglableScripts;

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

            m_MainCamera.fieldOfView = m_IsFirstPerson ? 80 : m_MainCamera.fieldOfView;
            m_MainCamera.transform.localPosition = m_IsFirstPerson ? new Vector3(0, 0.5f, 0) : m_MainCamera.transform.localPosition;

            foreach (GameObject target in m_ThirdPersonTogglables)
                target.SetActive(!m_IsFirstPerson);

            foreach (MonoBehaviour target in m_ThirdPersonTogglableScripts)
                target.enabled = !m_IsFirstPerson;

            foreach (GameObject target in m_FirstPersonTogglables)
                target.SetActive(m_IsFirstPerson);

            foreach (MonoBehaviour target in m_FirstPersonTogglableScripts)
                target.enabled = m_IsFirstPerson;
        }
    }
}
