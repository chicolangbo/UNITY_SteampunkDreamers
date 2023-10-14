using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BirdController : MapObject
{
    public AudioClip birdCryAudioClip;
    public AudioClip bumpAudioClip;
    public float planeSpeedDownRatio;
    public float planeSpeedUpRatio;

    public void Start()
    {
        onDisappear += () =>
        {
            ReleaseObject();
        };
    }

    public override void CollideEffect()
    {
        if(!playerController.shieldOn)
        {
            audioSource.PlayOneShot(birdCryAudioClip);
            audioSource.PlayOneShot(bumpAudioClip);

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
        else
        {
            playerController.shieldOn = false;
        }
    }
}