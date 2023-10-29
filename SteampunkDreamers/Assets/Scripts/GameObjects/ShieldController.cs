using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MapObject
{
    private void Start()
    {
        onDisappear += () =>
        {
            playerController.shieldOn = true;
            playerController.once = true;
            ReleaseObject();
        };
    }
}
