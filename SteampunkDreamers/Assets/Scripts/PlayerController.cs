using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float speedUpChange;
    public float speedDownChange;
    public float altitude = 1f;
    public float altitudeRatio = 100f;
    public float angleUpChange;
    public float angleDownChange;
    private Quaternion angleMin = Quaternion.Euler(0, 0, -60f);
    private Quaternion angleMax = Quaternion.Euler(0, 0, 60f);
    private float distance = 0f;

    private void Awake()
    {
    }

    private void Start()
    {
        StartCoroutine(DistanceLog());
    }

    private void FixedUpdate()
    {
        MovePlane(Input.GetMouseButton(0));
    }

    private void MovePlane(bool up)
    {
        Quaternion newAngle;
        float newSpeed;

        if (up)
        {
            newAngle = transform.rotation * Quaternion.Euler(0, 0, angleUpChange);
            newSpeed = Mathf.Clamp(speed - speedUpChange, 0, speed - speedUpChange);            
        }
        else
        {
            newAngle = transform.rotation * Quaternion.Euler(0, 0, -angleDownChange);
            newSpeed = Mathf.Clamp(speed + speedUpChange, 0, speed + speedUpChange);
        }
        speed = Mathf.Lerp(speed, newSpeed, Time.deltaTime);
        altitude = transform.rotation.z * speed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, newAngle, Time.deltaTime);
        distance += speed * Time.deltaTime;
    }

    private IEnumerator DistanceLog()
    {
        while(true)
        {
            Debug.Log("distance : " + distance);
            yield return new WaitForSeconds(1f);
        }
    }
}
