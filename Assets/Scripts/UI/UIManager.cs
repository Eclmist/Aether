using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private GameObject[] m_PowerUps;

    [SerializeField]
    private GameObject[] m_Items;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivatePowerupIcon(int index)
    {
        GameObject powerupIcon = m_PowerUps[index];
        UIPowerUpTimer timer = powerupIcon.GetComponent<UIPowerUpTimer>();
        if (timer != null)
        {
            timer.Activate();
        }
    }

    public void ActivateItem(int index)
    {
        GameObject itemIcon = m_Items[index];
        UIItemHandler handler = itemIcon.GetComponent<UIItemHandler>();
        if (handler != null)
        {
            handler.ReduceInstances();
        }
    }
}
