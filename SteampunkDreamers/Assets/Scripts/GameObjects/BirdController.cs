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

        //if()
        //var tempSpeed = playerController.frontSpeed - playerController.maxSpeed * effectStrength;
        //playerController.frontSpeed = (tempSpeed < 0)? 0 : tempSpeed;
    }

    public override void OnDie()
    {
        ReleaseObject();
    }


}