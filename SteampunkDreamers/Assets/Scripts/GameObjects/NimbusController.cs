using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NimbusController : MapObject
{
    public AudioClip electronicAudioClip;
    public int effectMaxCount;
    public float duration;
    private float elapsedTime = 0.0f;

    public void Start()
    {
        onDisappear += () =>
        {
            //playerController.electronicParticle.Play();
            //playerController.electronicParticle.gameObject.GetComponent<AudioSource>().enabled = true;
        };
    }

    public override void CollideEffect()
    {
        StartCoroutine(PlaneRotImpossible());
    }

    private IEnumerator PlaneRotImpossible()
    {
        StateGliding stateGliding = (StateGliding)playerController.stateMachine.CurrentState;

        if (stateGliding != null && !playerController.shieldOn)
        {
            playerController.electronicParticle.Play();
            playerController.electronicParticle.gameObject.GetComponent<AudioSource>().enabled = true;

            while (elapsedTime < duration)
            {
                stateGliding.isRotPossible = false;
                yield return null;
                elapsedTime += Time.deltaTime;
            }
            stateGliding.isRotPossible = true;
            SoundManager.instance.StopAudio();
            playerController.electronicParticle.Stop();
            playerController.electronicParticle.gameObject.GetComponent<AudioSource>().enabled = false;
        }
        else
        {
            //playerController.shieldOn = false;
        }

    }
}
