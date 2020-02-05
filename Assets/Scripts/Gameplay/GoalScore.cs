using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GoalScore : MonoBehaviour
{
    private Text m_GoalsScoredText;

    private int m_GoalsScored;
    // Start is called before the first frame update
    void Start()
    {
        m_GoalsScoredText = GetComponent<Text>();
        m_GoalsScored = GameManager.Instance.GoalsScoredRed;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GoalsScored != GameManager.Instance.GoalsScoredRed)
        {
            m_GoalsScoredText.text = GameManager.Instance.GoalsScoredRed.ToString();
        }
    }
}
