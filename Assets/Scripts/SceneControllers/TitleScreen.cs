using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    private Animator m_MainCanvasAnimator;

    [SerializeField]
    private GameObject[] m_DebugObjects;

    [SerializeField]
    private Animator m_ScreenFadeAnimator;

    [SerializeField]
    private MultiplayerMenu m_ForgeMultiplayerMenu;

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

        // TODO: Implement cheat manager
        if (Input.GetKeyDown(KeyCode.F1))
        {
            foreach (GameObject target in m_DebugObjects)
            {
                target.SetActive(true);
            }
        }
    }

    public void GoToMultiplayLobby()
    {
        AudioManager.m_Instance.PlaySoundAtPosition("GEN_Success_1", Camera.main.transform.position, 1.0f, 1.0f);
        m_ScreenFadeAnimator.SetTrigger("ToBlack");
        StartCoroutine("HostLobby");
    }

    private IEnumerator HostLobby()
    {
        yield return new WaitForSeconds(1.5f);
        m_ForgeMultiplayerMenu.Host();
    }

    public void LoadVisibilityGym()
    {
        SceneManager.LoadScene("GYM_Visibility 1");
        AudioManager.m_Instance.PlaySoundAtPosition("GEN_Success_1", Camera.main.transform.position, 1.0f, 1.0f);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
