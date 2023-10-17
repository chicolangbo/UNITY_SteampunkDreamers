using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBarController : MonoBehaviour
{
    //public Image fillBar;
    private Slider fillBar;
    private PlayerController playerController;
    public float value = 0;
    public float accelerator;
    private bool toRight;
    private float controllSpeed = 0.66f;

    private void Start()
    {
        fillBar = GetComponent<Slider>();
        playerController = GameManager.instance.player.GetComponent<PlayerController>();
    }
    public void SpeedBarMoving()
    {
        if (toRight)
        {
            // 1.5초 도달
            float tempValue = fillBar.value;
            fillBar.value = Mathf.Clamp(tempValue + Time.deltaTime * controllSpeed, 0, 1f);

            if (fillBar.value >= 1f)
            {
                toRight = false;
            }
        }
        // 1->0
        else
        {
            float tempValue = fillBar.value;
            fillBar.value = Mathf.Clamp(tempValue - Time.deltaTime * controllSpeed, 0, 1f);

            if (fillBar.value <= 0)
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
        if (fillBar.value < 0.7)
        {
            playerController.initialSpeed = playerController.maxSpeed * 0.5f;
        }
        else if ((fillBar.value >= 0.7 && fillBar.value < 0.8) || (fillBar.value >= 0.9 && fillBar.value <= 1))
        {
            playerController.initialSpeed = playerController.maxSpeed * 0.7f;
        }
        else if (fillBar.value >= 0.8 && fillBar.value < 0.9)
        {
            playerController.initialSpeed = playerController.maxSpeed * 0.9f;
        }
        GameManager.instance.SetBoardLength(playerController.initialSpeed);
        accelerator = Mathf.Pow(playerController.initialSpeed, 2) / ((GameManager.instance.boardScaleX - 20f) * 2);
        playerController.velocity += new Vector3(accelerator * Time.deltaTime, 0, 0);
    }
}
