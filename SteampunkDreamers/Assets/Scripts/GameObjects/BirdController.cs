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
        // 오디오클립 할당
    }

    public override void CollideEffect()
    {
        audioSource.PlayOneShot(birdCryAudioClip);
        audioSource.PlayOneShot(bumpAudioClip);

        // 1번 테스트
        // 각도 - 30~80 현재 속도 -= 맥스속도의 40 %
        // 각도 - 80~-30 현재 속도 += 맥스 속도의 40 %
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