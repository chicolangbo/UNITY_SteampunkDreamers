using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NimbusController : MapObject
{
    public AudioClip electronicAudioClip;

    public int effectMaxCount;

    public float duration;
    private float elapsedTime = 0.0f;

    public override void CollideEffect()
    {
        StartCoroutine(PlaneRotImpossible());
    }

    private IEnumerator PlaneRotImpossible()
    {
        StateGliding stateGliding = (StateGliding)playerController.stateMachine.CurrentState;

        if (stateGliding != null && !playerController.shieldOn)
        {
            while (elapsedTime < duration)
            {
                stateGliding.isRotPossible = false;
                yield return null;
                elapsedTime += Time.deltaTime;
            }
            stateGliding.isRotPossible = true;
        }
        else
        {
            playerController.shieldOn = false;
        }

    }
}
