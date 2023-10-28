using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipController : MapObject
{
    public AudioClip explisionAudioClip;
    private GameObject explosion;

    public void Start()
    {
        onDisappear += () =>
        {
            explosion = ObjectPoolManager.instance.GetGo("AirshipParticle");
            explosion.transform.position = transform.position;
            explosion.GetComponent<ParticleAutoRelease>().PlayAndRelease();
            audioSource.PlayOneShot(explisionAudioClip);
            playerController.explosionParticle.Play();
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

    IEnumerator WaitAndReleaseObject(float waitTime)
    {
        // 사운드 재생 시간 동안 대기
        yield return new WaitForSeconds(waitTime);

        // 대기 시간이 끝나면 ReleaseObject() 호출
        ReleaseObject();
    }
}
