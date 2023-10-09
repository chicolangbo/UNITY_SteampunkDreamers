using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum AirflowType
{
    Front,
    Reverse
}

public class AirflowSystem : MonoBehaviour
{
    public AirflowType airflowType;
    public float waitTime = 5f;
    private float timer = 0f;
    private bool once = true;
    private GameObject airflowFront;
    private GameObject airflowReverse;
    private FadeController fadeEffect;
    public event Action onDisappear;
    public float maxSpeed;
    public PlayerController playerController;

    public void Awake()
    {
        //Destroy(gameObject, waitTime);
        StartCoroutine(Disappear());
        airflowFront = transform.GetChild(0).gameObject;
        airflowReverse = transform.GetChild(1).gameObject;
        fadeEffect = GetComponent<FadeController>();
    }

    public void Start()
    {
        fadeEffect.StartFadeIn();
    }

    public void Update()
    {
        // fade out
        timer += Time.deltaTime;
        if(timer > waitTime - fadeEffect.fadeTime && once)
        {
            fadeEffect.StartFadeOut();
            once = false;
        }
    }

    public void Setup(AirflowType type)
    {
        airflowType = type;
        if(type == AirflowType.Front)
        {
            airflowFront.SetActive(true);
            airflowReverse.SetActive(false);
            fadeEffect.material = airflowFront.GetComponent<MeshRenderer>().material;
        }
        else
        {
            airflowFront.SetActive(false);
            airflowReverse.SetActive(true);
            fadeEffect.material = airflowReverse.GetComponent<MeshRenderer>().material;
        }

    }

    public IEnumerator Disappear()
    {
        yield return new WaitForSeconds(waitTime);
        onDisappear();
    }
}