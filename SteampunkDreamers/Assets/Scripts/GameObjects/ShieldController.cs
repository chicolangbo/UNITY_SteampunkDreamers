using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MapObject
{
    public AudioClip shieldGetClip;

    private void Start()
    {
        onDisappear += () =>
        {
            playerController.shieldOn = true;
            playerController.once = true;
            SoundManager.instance.PlaySingleAudio(shieldGetClip);
            ReleaseObject();
        };
    }
}
