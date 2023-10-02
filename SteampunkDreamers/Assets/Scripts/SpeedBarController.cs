using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBarController : MonoBehaviour
{
    public Image fillBar;
    public RectTransform controller;
    public float speedValue = 0;

    public void Update()
    {
        SpeedChange(speedValue);
    }

    public void SpeedChange(float value)
    {
        if (value > 100 || value < 0)
            return;

        float amount = (value / 100.0f) * 180.0f / 360;
        fillBar.fillAmount = amount;

        float controllerAngle = amount * 360;
        controller.localEulerAngles = new Vector3(0,0,-controllerAngle);
    }
}
