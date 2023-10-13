using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBarController : MonoBehaviour
{
    public Image fillBar;
    public RectTransform arrow;
    private PlayerController playerController;
    public float value = 0;
    public float accelerator;
    private bool toRight;
    private float controllSpeed = 66.67f;

    private void Start()
    {
        playerController = GameManager.instance.player.GetComponent<PlayerController>();
    }

    public void Update()
    {
        SpeedChange(value);
    }

    public void SpeedChange(float value)
    {
        if (value > 100 || value < 0)
            return;

        float amount = (value / 100.0f) * 180.0f / 360;
        fillBar.fillAmount = amount;

        float controllerAngle = amount * 360;
        arrow.localEulerAngles = new Vector3(0,0,-controllerAngle);
    }

    public void SpeedBarMoving()
    {
        if (toRight)
        {
            // 1.5초 도달
            float tempValue = value;
            value = Mathf.Clamp(tempValue + Time.deltaTime * controllSpeed, 0, 100f);

            if (value >= 100f)
            {
                toRight = false;
            }
        }
        // 100->0
        else
        {
            float tempValue = value;
            value = Mathf.Clamp(tempValue - Time.deltaTime * controllSpeed, 0, 100f);

            if (value <= 0)
            {
                toRight = true;
            }
        }
    }

    public void SetVelocity()
    {
        // 최초(최대) 속력 세팅
        // value 0~70 : 최대 속력의 50%
        // value 70~80, 90~100 : 최대 속력의 70%
        // vlaue 80~90 : 최대 속력의 90%
        if (value < 70)
        {
            playerController.initialSpeed = playerController.maxSpeed * 0.5f;
        }
        else if ((value >= 70 && value < 80) || (value >= 90 && value <= 100))
        {
            playerController.initialSpeed = playerController.maxSpeed * 0.7f;
        }
        else if (value >= 80 && value < 90)
        {
            playerController.initialSpeed = playerController.maxSpeed * 0.9f;
        }
        GameManager.instance.SetBoardLength(playerController.initialSpeed);
        accelerator = Mathf.Pow(playerController.initialSpeed, 2) / ((GameManager.instance.boardScaleX - 20f) * 2);
        playerController.velocity += new Vector3(accelerator * Time.deltaTime, 0, 0);
    }
}
