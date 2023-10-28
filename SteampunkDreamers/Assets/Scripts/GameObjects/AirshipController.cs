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
