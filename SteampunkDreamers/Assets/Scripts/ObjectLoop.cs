using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLoop : MonoBehaviour
{
    private int loopWave = 0;

    private Transform player;
    private Vector3 loopPoint;

    private float width;

    public void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        width = transform.localScale.x;
        loopPoint = transform.position;
    }

    public void FixedUpdate()
    {
        if(player.position.x > transform.position.x + width)
        {
            loopPoint.x = transform.position.x + width * 2f;
            transform.position = loopPoint;
        }

        //if ((int)player.position.x / 3000 != loopWave)
        //{
        //    loopWave = (int)player.position.x / 3000;
        //    loopPoint.x = 3000f * loopWave - 20f;
        //}
    }
}
