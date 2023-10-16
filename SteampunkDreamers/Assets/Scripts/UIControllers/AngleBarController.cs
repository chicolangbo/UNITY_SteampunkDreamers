using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngleBarController : MonoBehaviour
{
    private Slider fillBar;
    private PlayerController playerController;
    public float value = 0;
    private bool toRight;
    private float controllSpeed = 0.33f;

    private void Start()
    {
        playerController = GameManager.instance.player.GetComponent<PlayerController>();
        fillBar = gameObject.GetComponent<Slider>();
    }

    public void AngleBarMoving()
    {
        // 3초 도달
        if (fillBar != null)
        {
            if (!toRight)
            {
                float tempValue = fillBar.value;
                fillBar.value = Mathf.Clamp(tempValue + Time.deltaTime * controllSpeed, 0, 1f);
                if (fillBar.value >= 1f)
                {
                    toRight = true;
                }
            }
            else
            {
                float tempValue = fillBar.value;
                fillBar.value = Mathf.Clamp(tempValue - Time.deltaTime * controllSpeed, 0, 1f);
                if (fillBar.value <= 0f) // 순회 완료
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public void SetAngle()
    {
        // 최초 각도 세팅
        // value 0~60, 91~100 : 1부터 시작
        // value 61~70, 81~90
        // value 71~80
        playerController.initialAngle = new Quaternion(0, 0, (fillBar.value < 0.1) ? 1f - 30f : fillBar.value * 100f - 30f, 1);
    }
}
