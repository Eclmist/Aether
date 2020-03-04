using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconsManager : Singleton<IconsManager>
{

    public Image GetIcon(int index)
    {
        return gameObject.transform.GetChild(index).GetComponent<Image>();
    }
}
