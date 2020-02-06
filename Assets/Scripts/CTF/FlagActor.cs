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
                SetCapture(c.gameObject);
                manager.SetFlagInPosession(true);
                manager.SetPlayerInPosession(c.gameObject);
                //Destroy(gameObject);
            }
        }
    }

    private void SetCapture(GameObject player)
    {
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
        //gameObject.GetComponent<AiActor>().SetActive();
    }
    
}
