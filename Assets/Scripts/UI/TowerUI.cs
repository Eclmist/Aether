using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour
{
    public GameObject ui;
    public Image fill;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetCurrentTower() == null)
        {
            ui.SetActive(false);
        }

        ui.SetActive(true);
        fill.fillAmount = GameManager.Instance.GetCurrentTower().GetCapturePercentage();
    }
}
