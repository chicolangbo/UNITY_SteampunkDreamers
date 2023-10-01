using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLoop : MonoBehaviour
{
    public Vector3 respawnPos;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Respawn"))
        {
            transform.position = respawnPos;
        }
    }
}
