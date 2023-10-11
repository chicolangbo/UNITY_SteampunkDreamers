using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : Obstacle
{
    private AudioClip birdCryAudioClip;
    private AudioClip bumpAudioClip;

    public void Start()
    {
        // �����Ŭ�� �Ҵ�
    }

    public override void CollideEffect()
    {
        audioSource.PlayOneShot(birdCryAudioClip);
        audioSource.PlayOneShot(bumpAudioClip);

        // �ӵ� -= �ƽ��ӵ��� 40%
    }

    public override void OnDie()
    {
        Debug.Log("�浹");
        ReleaseObject();
    }


}