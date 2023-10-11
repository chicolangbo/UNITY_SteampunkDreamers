using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObj : MonoBehaviour
{
    public float speed;
    public PlayerController player;

    private void FixedUpdate()
    {
        //speed = player.speed;
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
