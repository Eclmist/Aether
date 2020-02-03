using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    public Animator m_MainCanvasAnimator;

    private bool m_AnyKeyPressed;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && m_MainCanvasAnimator != null && !m_AnyKeyPressed)
        {
            // Ignore mouse input 
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                return;

            m_MainCanvasAnimator.SetTrigger("Start");
            AudioManager.m_Instance.PlaySoundAtPosition("GEN_Success_2", Camera.main.transform.position, 1.0f, 1.0f);
            m_AnyKeyPressed = true;
        }
    }

    public void GoToMultiplayLobby()
    {
        AudioManager.m_Instance.PlaySoundAtPosition("GEN_Success_1", Camera.main.transform.position, 1.0f, 1.0f);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
