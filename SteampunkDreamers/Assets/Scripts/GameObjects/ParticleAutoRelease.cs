using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoRelease : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void PlayAndRelease()
    {
        particle.Play();
        StartCoroutine(WaitAndRelease(particle.main.duration));
    }

    IEnumerator WaitAndRelease(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.GetComponent<PoolAble>().ReleaseObject();
    }
}
