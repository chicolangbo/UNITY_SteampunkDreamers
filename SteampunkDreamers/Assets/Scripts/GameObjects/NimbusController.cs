using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NimbusController : Obstacle
{
    public AudioClip electronicAudioClip;
    public ParticleSystem electronicParticle;
    public int effectMaxCount;

    public override void CollideEffect()
    {
        if(effectMaxCount != 0)
        {
            effectMaxCount--;

        }
    }

    private IEnumerator PlaneRotImpossible()
    {
        while(effectMaxCount > 0)
        {
            string stateName = playerController.stateMachine.GetCurrentState();

            if(playerController.stateMachine.currentSatateName == "Gliding")
            {
                StateGliding stateGliding = (StateGliding)playerController.stateMachine.CurrentState;
                stateGliding.isRotPossible = false;
            }

            yield return null;
            ++effectMaxCount;
        }
    }
}
