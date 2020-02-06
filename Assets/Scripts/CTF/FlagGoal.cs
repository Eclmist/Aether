using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlagGoal : MonoBehaviour
{
    public bool isRedGoal;
    
    void OnTriggerEnter(Collider c) 
    {
        if (c.CompareTag("Player"))
        {
            FlagManager manager = c.GetComponent<FlagManager>();
            if (manager != null && manager.CheckIfFlagInPosession() /* && CheckPlayer(c.gameObject) */)
            {
                IndicateVictory(c);
                manager.SetFlagInPosession(false);
            }
        }
    }

    //private bool CheckPlayer(GameObject player)
    //{
    //    if (isRedGoal)
    //    {
    //        return GameManager.Instance.playersInTeamRed.Contains(player);
    //    }
    //    else
    //    {
    //        return GameManager.Instance.playersInTeamBlue.Contains(player);
    //    }
    //}

    private void IndicateVictory(Collider c)
    {
        c.GetComponent<PlayerAnimation>().TriggerVictoryAnimation();
        //c.GetComponent<PlayerMovement>().SetUnmovable(true); 
        GameManager.Instance.IncrementScore(isRedGoal);
        c.GetComponentInChildren<FlagActor>().LetGo();
        MultiplayerSceneController.Instance.UINotify("Red Team Wins!");
        AudioManager.m_Instance.PlaySound("GEN_Achievement_2", 1.0f, 1.0f);

    }

}
