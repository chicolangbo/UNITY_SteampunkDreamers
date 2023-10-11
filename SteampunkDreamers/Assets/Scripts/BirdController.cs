using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : Obstacle
{
    private void Start()
    {
        name = "Bird";
        type = EffectType.Speed;
        effectStrength = 0.3f;
    }
}