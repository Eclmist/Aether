using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GoalScore : MonoBehaviour
{
    private Text m_GoalsScoredText;
    public bool isRed;

    private int m_GoalsScored;
    // Start is called before the first frame update
    void Start()
    {
        m_GoalsScoredText = GetComponent<Text>();
        if(isRed)
            m_GoalsScored = GameManager.Instance.GoalsScoredRed;
        else
        {
            m_GoalsScored = GameManager.Instance.GoalsScoredBlue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isRed)
        {
            if (m_GoalsScored != GameManager.Instance.GoalsScoredRed)
            {
                m_GoalsScoredText.text = GameManager.Instance.GoalsScoredRed.ToString();
            }
        }
        else
        {
            if (m_GoalsScored != GameManager.Instance.GoalsScoredBlue)
            {
                m_GoalsScoredText.text = GameManager.Instance.GoalsScoredBlue.ToString();
            }
        }
    }
}
