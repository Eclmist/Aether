using System.Collections.Generic;
using UnityEngine;

public class PowerupHandler : MonoBehaviour
{
    private Dictionary<int, bool> m_PowerupMap;

    void Awake()
    {
        m_PowerupMap = new Dictionary<int, bool>();
        m_PowerupMap.Add(0, false);
        m_PowerupMap.Add(1, false);
    }

    public bool IsBoosted(int id) {
        return m_PowerupMap[id];
    }

    public void SetBoosted(int id, bool value)
    {
        if (m_PowerupMap.ContainsKey(id))
            m_PowerupMap[id] = value;
    }
}
