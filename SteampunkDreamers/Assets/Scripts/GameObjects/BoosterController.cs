using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterController : MapObject
{
    private void Start()
    {
        onDisappear += () =>
        {
            playerController.fireParticle.Play();
            playerController.fireParticle.gameObject.GetComponent<AudioSource>().enabled = true;
            ReleaseObject();
        };
    }
    public override void CollideEffect()
    {
        playerController.boosterOn = true;
        playerController.boosterSpeed = playerController.frontSpeed * 0.2f;
    }
}
