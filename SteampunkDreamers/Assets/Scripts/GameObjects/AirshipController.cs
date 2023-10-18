using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipController : MapObject
{
    public AudioClip fireAudioClip;
    public AudioClip explisionAudioClip;
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
            playerController.airshipColiide = true;

            StartCoroutine(ClickImpossible());
        }
        else
        {
            playerController.shieldOn = false;
        }
    }

    private IEnumerator ClickImpossible()
    {
        while (true)
        {
            StateGliding stateGliding = (StateGliding)playerController.stateMachine.CurrentState;

            if (stateGliding != null)
            {
                stateGliding.isRotPossible = false;
            }
            else
            {
                break; // stateGliding이 null이면 루프 종료
            }

            yield return null;
        }
    }
}
