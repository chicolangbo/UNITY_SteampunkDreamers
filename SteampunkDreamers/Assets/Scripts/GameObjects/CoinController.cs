using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MapObject
{
    public AudioClip coinAudioClip;

    public void Start()
    {
        onDisappear += () =>
        {
            playerController.coinCount++;
            ReleaseObject();
        };
    }

    //public override void CollideEffect()
    //{
    //    audioSource.PlayOneShot(coinAudioClip);
    //    playerController.coinCount++;
    //}
}
