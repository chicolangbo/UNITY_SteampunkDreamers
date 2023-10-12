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
        playerController.isCollideBird = true;
        Debug.Log("true");
        playerController.SetFalseEffect();

        // 1�� �׽�Ʈ
        // ���� -30~80 ���� �ӵ� -= �ƽ��ӵ��� 40%
        // ���� -80~-30 ���� �ӵ� += �ƽ� �ӵ��� 40%
        //var angle = Utils.EulerToAngle(playerController.transform.localEulerAngles.z);
        //var anglePercentage = (angle - playerController.minAngle) / (playerController.maxAngle - playerController.minAngle) * 100f;

        //if (anglePercentage >= 30)
        //{
        //    playerController.frontSpeed -= playerController.maxSpeed * 0.4f;
        //}
        //else
        //{
        //    playerController.frontSpeed += playerController.maxSpeed * 0.4f;
        //}

        // 2�� �׽�Ʈ
        // �����̼� �������� �ӵ��� ������?


    }

    public override void OnDie()
    {
        //playerController.isCollideBird = false;
        ReleaseObject();
    }
}