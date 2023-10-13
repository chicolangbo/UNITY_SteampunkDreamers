using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipController : Obstacle
{
    public AudioClip fireAudioClip;
    public AudioClip explisionAudioClip;
    public void Start()
    {
        onDisappear += () =>
        {
            ReleaseObject();
        };
    }

    public override void CollideEffect()
    {
        playerController.airshipColiide = true;

        StartCoroutine(ClickImpossible());
    }

    private IEnumerator ClickImpossible()
    {
        while (true)
        {
            StateGliding stateGliding = (StateGliding)playerController.stateMachine.CurrentState;

            Debug.Log(playerController.stateMachine.GetCurrentState());

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
}
