using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BirdSpawner : Spawner
{
    public BirdController birdPrefab;
    public float playerBehindLength = 10f;
    private float minSpeed = 0.4f;
    private float maxSpeed = 0.6f;

    public void Start()
    {
        spawnRangeValue = 3;
        spawnDelayTime = 3f;
        StartCoroutine(CreateBird());
    }

    public IEnumerator CreateBird()
    {
        while (!spawnStop)
        {
            if (GameManager.instance != null && GameManager.instance.isGameover)
            {
                break;
            }

            FindPlayerIndex();
            GetRandomSpawnIndex();
            Debug.Log(playerIndex);
            BirdController birdGo = ObjectPoolManager.instance.GetGo("Bird").GetComponent<BirdController>();
            // speed
            var randomValue = Random.Range(minSpeed, maxSpeed);
            birdGo.speed = playerController.frontSpeed * randomValue;
            // position
            selectedPoint.x = playerController.transform.position.x - playerBehindLength;
            birdGo.rb.position = selectedPoint;

            yield return new WaitForSeconds(spawnDelayTime);
            spawnDelayTime = Random.Range(0.3f, 4f);
        }
    }
}
