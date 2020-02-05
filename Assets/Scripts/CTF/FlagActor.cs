using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlagActor : MonoBehaviour
{
    private Vector3 m_SpawnPos;

    private void Start()
    {
        m_SpawnPos = this.gameObject.transform.position;
    }
    
    void OnTriggerEnter(Collider c) 
    {
        if (c.CompareTag("Player"))
        {
            SetCapture(c.gameObject);
            FlagManager manager = c.GetComponent<FlagManager>();

            if (manager != null && !manager.CheckIfFlagInPosession())
            {
                manager.SetBool(true);
                //Destroy(gameObject);
            }
        }
    }

    private void SetCapture(GameObject player)
    {
        gameObject.transform.SetParent(player.transform);
        //gameObject.GetComponent<AiActor>().SetInactive();
    }

    public void LetGo()
    {
        gameObject.transform.SetParent(null);
        gameObject.transform.position = m_SpawnPos;
        //gameObject.GetComponent<AiActor>().SetActive();
    }
    
}
