using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateLanding : BaseState
{
    public StateLanding(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        controller.velocity.y = 0f;

    }

    public override void OnExitState()
    {
    }

    public override void OnFixedUpdateState()
    {
        // speed -> 0
        controller.velocity.x = Mathf.Lerp(controller.velocity.x, 0, Time.deltaTime * 2f);

        // position.y -> 0
        var tempY = controller.transform.position.y;
        tempY = Mathf.Clamp(controller.transform.position.y - Time.deltaTime, 0, controller.transform.position.y);
        controller.transform.position = new Vector3(controller.transform.position.x,tempY,0);

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
