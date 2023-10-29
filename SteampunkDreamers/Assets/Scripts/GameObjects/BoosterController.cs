using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterController : MapObject
{
    public AudioClip fireAudioClip;

    private void Start()
    {
        onDisappear += () =>
        {
            SoundManager.instance.PlayLoopAudio(fireAudioClip);
            playerController.fireParticle.Play();
            ReleaseObject();
        };
    }
    public override void CollideEffect()
    {
        playerController.boosterOn = true;
        playerController.once = true;
        playerController.boosterSpeed = playerController.frontSpeed * 0.2f;
    }
}
