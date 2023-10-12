using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : Obstacle
{
    private AudioClip birdCryAudioClip;
    private AudioClip bumpAudioClip;

    public void Start()
    {
        // 오디오클립 할당
    }

    public override void CollideEffect()
    {
        audioSource.PlayOneShot(birdCryAudioClip);
        audioSource.PlayOneShot(bumpAudioClip);
        playerController.isCollideBird = true;
        Debug.Log("true");
        playerController.SetFalseEffect();

        // 1번 테스트
        // 각도 -30~80 현재 속도 -= 맥스속도의 40%
        // 각도 -80~-30 현재 속도 += 맥스 속도의 40%
        //var angle = Utils.EulerToAngle(playerController.transform.localEulerAngles.z);
        //var anglePercentage = (angle - playerController.minAngle) / (playerController.maxAngle - playerController.minAngle) * 100f;

        //if (anglePercentage >= 30)
        //{
        //    playerController.frontSpeed -= playerController.maxSpeed * 0.4f;
        //}
        //else
        //{
        //    playerController.frontSpeed += playerController.maxSpeed * 0.4f;
        //}

        // 2번 테스트
        // 로테이션 내려가는 속도를 빠르게?


    }

    public override void OnDie()
    {
        //playerController.isCollideBird = false;
        ReleaseObject();
    }
}