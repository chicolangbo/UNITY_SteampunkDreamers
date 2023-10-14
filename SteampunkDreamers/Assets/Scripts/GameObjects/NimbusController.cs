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
                Debug.Log("로테이션 막음");
                stateGliding.isRotPossible = false;
                yield return null;
                elapsedTime += Time.deltaTime;
            }
        }
        else
        {
            playerController.shieldOn = false;
        }

        stateGliding.isRotPossible = true;
    }
}
