using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlagActor : MonoBehaviour
{
    private Vector3 m_SpawnPos;
    private Vector3 m_SpawnRotation;
    public Vector3 m_PosWhenCaptured;
    public Vector3 m_RotationWhenCaptured;
    public GameObject m_HighlightObject;


    private void Start()
    {
        m_SpawnPos = this.gameObject.transform.position;
    }
    
    void OnTriggerEnter(Collider c) 
    {
        if (c.CompareTag("Player"))
        {
            
            FlagManager manager = c.GetComponent<FlagManager>();

            if (manager != null && !manager.CheckIfFlagInPosession())
            {
                m_HighlightObject.SetActive(false);
                SetCapture(c.gameObject);
                manager.SetFlagInPosession(true);
                manager.SetPlayerInPosession(c.gameObject);
                //Destroy(gameObject);
            }
        }
    }

    private void SetCapture(GameObject player)
    {
        MultiplayerSceneController.Instance.UINotify("Flag Captured");
        AudioManager.m_Instance.PlaySound("GEN_Achievement_2", 1.0f, 1.0f);

        Transform[] childTransform = player.transform.GetComponentsInChildren<Transform>();
        bool skipPastHairBone = false;
        foreach (Transform t in childTransform)
        {
            if (t.name == "Character1_RightHand")
            {
                if (!skipPastHairBone)
                {
                    skipPastHairBone = true;
                    continue;
                }

                gameObject.transform.SetParent(t);
                gameObject.transform.localPosition = new Vector3(0.03f, 0.06f, 0); //E3: Hard coded values for now
                gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 180));
                return;
            }
        }


        gameObject.transform.SetParent(player.transform);
        gameObject.transform.localPosition = m_PosWhenCaptured;
        gameObject.transform.localEulerAngles = m_RotationWhenCaptured;
        //gameObject.GetComponent<AiActor>().SetInactive();
    }

    public void LetGo()
    {
        gameObject.transform.SetParent(null);
        gameObject.transform.position = m_SpawnPos;
        gameObject.transform.eulerAngles = m_SpawnRotation;
        gameObject.transform.localScale = Vector3.one;
        m_HighlightObject.SetActive(true);
        //gameObject.GetComponent<AiActor>().SetActive();
    }

}
