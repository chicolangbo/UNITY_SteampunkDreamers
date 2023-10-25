using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TouchTest : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Update is called once per frame
    void Update()
    {
        var message = string.Empty;
        foreach(Touch touch in Input.touches)
        {
            message += "Touch ID: " + touch.fingerId;
            message += "\nPhase" + touch.phase;
            message += "\nPosition: " + touch.position;
            message += "\nDelta Time: " + touch.deltaTime;
            message += "\nDelta Pos: " + touch.deltaPosition + "\n";
        }
        message += "\n";
        text.text = message;

        Debug.Log(Input.touchCount);
        foreach(Touch touch in Input.touches)
        {
            Debug.Log(touch.fingerId);
        }
        Debug.Log("");
    }
}
