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
        // speed -> 0
        controller.initialFrontSpeed = Mathf.Lerp(controller.initialFrontSpeed, 0, Time.deltaTime);

        // rotation -> 0
        if (controller.transform.rotation.z > 0.5f || controller.transform.rotation.z < -0.5f)
        {
            controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, new Quaternion(0, 0, 0, 0), Time.deltaTime);
        }
    }

    public override void OnUpdateState()
    {
    }

}
