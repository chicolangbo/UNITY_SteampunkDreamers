using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLanding : BaseState
{
    public StateLanding(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
    }

    public override void OnExitState()
    {
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnUpdateState()
    {
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            controller.initialFrontSpeed = Mathf.Lerp(controller.initialFrontSpeed, 0, Time.deltaTime);
        }
    }
}
