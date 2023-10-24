using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFitter : MonoBehaviour
{
    private void Awake()
    {
        var h = Screen.height;
        var w = Screen.width;
        var rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(w, h);
    }
}
