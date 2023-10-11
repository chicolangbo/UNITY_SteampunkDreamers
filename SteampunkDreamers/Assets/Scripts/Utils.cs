using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float EulerToAngle(float z)
    {
        return (z > 180f) ? z - 360f : z;
    }

    public static Quaternion ClampRotation(Vector3 localEulerAngle, float minAngle, float maxAngle)
    {
        localEulerAngle.z = EulerToAngle(localEulerAngle.z);
        localEulerAngle.z = Mathf.Clamp(localEulerAngle.z, minAngle, maxAngle);
        return Quaternion.Euler(localEulerAngle);
    }

}
