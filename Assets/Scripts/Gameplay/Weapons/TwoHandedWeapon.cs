using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Weapon assumes that it is already attached to primary hand
public class TwoHandedWeapon : MonoBehaviour
{
    [SerializeField]
    private Transform m_OtherHand;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - m_OtherHand.position, transform.parent.forward); 
    }
}
