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

        // 속도 -= 맥스속도의 40%
    }

    public override void OnDie()
    {
        Debug.Log("충돌");
        ReleaseObject();
    }


}