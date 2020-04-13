using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UXManager : Singleton<UXManager>
{
    [SerializeField]
    private Animator m_ScreenFadeAnimator;

    [SerializeField]
    private Text m_Text;

    [SerializeField]
    private int m_Iterations;

    public void Start() 
    {
        StartCoroutine(TriggerTextChange());
    } 

    private IEnumerator TriggerTextChange()
    {
        for (int i = 0; i < m_Iterations; i++)
        {
            m_Text.text = "LOADING";
            yield return new WaitForSeconds(0.5f);
            m_Text.text = "LOADING.";
            yield return new WaitForSeconds(0.5f);
            m_Text.text = "LOADING..";
            yield return new WaitForSeconds(0.5f);
            m_Text.text = "LOADING...";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void StartFade()
    {
        if (m_ScreenFadeAnimator == null)
            return;

        m_ScreenFadeAnimator.SetTrigger("ToBlack");
    }
}
