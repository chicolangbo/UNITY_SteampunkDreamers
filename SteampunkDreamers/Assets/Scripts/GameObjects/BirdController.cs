using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BirdController : Obstacle
{
    private AudioClip birdCryAudioClip;
    private AudioClip bumpAudioClip;
    public float planeSpeedDownRatio;
    public float planeSpeedUpRatio;

    public void Start()
    {
        // �����Ŭ�� �Ҵ�
    }

    public override void CollideEffect()
    {
        audioSource.PlayOneShot(birdCryAudioClip);
        audioSource.PlayOneShot(bumpAudioClip);

        // 1�� �׽�Ʈ
        // ���� - 30~80 ���� �ӵ� -= �ƽ��ӵ��� 40 %
        // ���� - 80~-30 ���� �ӵ� += �ƽ� �ӵ��� 40 %
       var angle = Utils.EulerToAngle(playerController.transform.localEulerAngles.z);
        var anglePercentage = (angle - playerController.minAngle) / (playerController.maxAngle - playerController.minAngle) * 100f;

        if (anglePercentage >= 30)
        {
            playerController.frontSpeed -= playerController.maxSpeed * planeSpeedDownRatio;
        }
        else
        {
            playerController.frontSpeed += playerController.maxSpeed * planeSpeedUpRatio;
        }
    }
}