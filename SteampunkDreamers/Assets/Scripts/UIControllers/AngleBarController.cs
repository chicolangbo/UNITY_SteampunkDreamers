using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngleBarController : MonoBehaviour
{
    public Image fillBar;
    public RectTransform arrow;
    private PlayerController playerController;
    public float value = 0;
    private bool toRight;
    private float controllSpeed = 33.33f;

    private void Start()
    {
        playerController = GameManager.instance.player.GetComponent<PlayerController>();
    }

    public void Update()
    {
        AngleChange(value);
    }

    public void AngleChange(float value)
    {
        if (value > 100 || value < 0)
            return;

        float amount = (value / 100.0f) * 120.0f / 360;
        fillBar.fillAmount = amount;

        float controllerAngle = amount * 360;
        arrow.localEulerAngles = new Vector3(0, 0, controllerAngle);
    }

    public void AngleBarMoving()
    {
        // 3초 도달
        if (!toRight)
        {
            float tempValue = value;
            value = Mathf.Clamp(tempValue + Time.deltaTime * controllSpeed, 0, 100f);
            if (value >= 100f)
            {
                toRight = true;
            }
        }
        else
        {
            float tempValue = value;
            value = Mathf.Clamp(tempValue - Time.deltaTime * controllSpeed, 0, 100f);
            if (value <= 0f) // 순회 완료
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void SetAngle()
    {
        // 최초 각도 세팅
        // value 0~60, 91~100 : 1부터 시작
        // value 61~70, 81~90
        // value 71~80
        playerController.initialAngle = new Quaternion(0, 0, (value < 1) ? 1f - 30f : value - 30f, 1);
    }
}
