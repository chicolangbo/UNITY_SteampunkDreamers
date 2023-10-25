using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFitter : MonoBehaviour
{
    private void Awake()
    {
        var h = Screen.height;
        var w = Screen.width;
        var freeSpace = 500f;
        var rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(w + freeSpace, h + freeSpace);
    }
}
