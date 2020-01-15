using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Aether.Prototype
{
    public class ReloadScene : MonoBehaviour
    {
        public KeyCode m_TargetKey;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(m_TargetKey))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
