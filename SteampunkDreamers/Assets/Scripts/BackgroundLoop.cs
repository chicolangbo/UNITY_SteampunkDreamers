using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    private float width;
    public bool child;
    private Transform player;

    public void Awake()
    {
        if(child)
        {
            width = GetComponentInChildren<Transform>().localScale.x;
        }
        else
        {
            width = GetComponent<BoxCollider>().size.x;
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        if(player.position.x - transform.position.x > width * 1.5f)
        {
            var tempPos = new Vector3(transform.position.x + 2 * width, transform.position.y, transform.position.z);
            transform.position = tempPos;
        }
    }
}
