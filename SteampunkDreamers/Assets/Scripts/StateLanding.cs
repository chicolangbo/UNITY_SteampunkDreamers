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
        controller.frontSpeed = Mathf.Lerp(controller.frontSpeed, 0, Time.deltaTime * 5f);

        // rotation -> 0
        var tempRot = controller.transform.localRotation;
        if(tempRot.z > 0)
        {
            tempRot.z = Mathf.Clamp(tempRot.z - Time.deltaTime, 0, tempRot.z);
        }
        else if(tempRot.z < 0)
        {
            tempRot.z = Mathf.Clamp(tempRot.z + Time.deltaTime, tempRot.z, 0);
        }
        controller.transform.localRotation = tempRot;
    }

    public override void OnUpdateState()
    {
    }
}
