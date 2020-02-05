using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagIconToggler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetIcon()
    {
        gameObject.SetActive(true);
    }

    public void ResetIcon()
    {
        gameObject.SetActive(false);
    }
}
