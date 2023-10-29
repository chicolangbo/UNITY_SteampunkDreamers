using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipController : MapObject
{
    public AudioClip explosionAudioClip;
    private GameObject explosion;

    public void Start()
    {
        onDisappear += () =>
        {
            //explosion = ObjectPoolManager.instance.GetGo("AirshipParticle");
            //explosion.transform.position = transform.position;
            //explosion.GetComponent<ParticleAutoRelease>().PlayAndRelease();
            //playerController.explosionParticle.Play();
            //SoundManager.instance.PlayAudioClip(explosionAudioClip);
            ReleaseObject();
        };
    }

    public override void CollideEffect()
    {
        if(!playerController.shieldOn)
        {
            explosion = ObjectPoolManager.instance.GetGo("AirshipParticle");
            explosion.transform.position = transform.position;
            explosion.GetComponent<ParticleAutoRelease>().PlayAndRelease();

            playerController.airshipColiide = true;
            playerController.explosionParticle.Play();
            SoundManager.instance.PlayAudioClip(explosionAudioClip);
            StartCoroutine(ClickImpossible());
        }
        else
        {
            //playerController.shieldOn = false;
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
                break; // stateGliding�� null�̸� ���� ����
            }

            yield return null;
        }
    }

    IEnumerator WaitAndReleaseObject(float waitTime)
    {
        // ���� ��� �ð� ���� ���
        yield return new WaitForSeconds(waitTime);

        // ��� �ð��� ������ ReleaseObject() ȣ��
        ReleaseObject();
    }
}
