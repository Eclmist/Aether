using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlagActor : MonoBehaviour
{
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
        GameObject parentGameObject = this.transform.parent.gameObject;
        parentGameObject.transform.SetParent(player.transform);
        parentGameObject.GetComponent<AiActor>().SetInactive();
    }

    public void LetGo()
    {
        GameObject parentGameObject = this.transform.parent.gameObject;
        parentGameObject.transform.SetParent(null);
        parentGameObject.transform.position = new Vector3();
        parentGameObject.GetComponent<AiActor>().SetActive();
    }
    
}
