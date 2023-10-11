using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleSpawner : Spawner
{
    public Obstacle prefab;
    public float playerBehindLength;
    public float minSpeed;
    public float maxSpeed;
    public float minSpawnDelayTime;
    public float maxSpawnDelayTime;

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
            Obstacle go = ObjectPoolManager.instance.GetGo(prefab.name).GetComponent<Obstacle>();
            // speed
            var randomValue = Random.Range(minSpeed, maxSpeed);
            go.speed = playerController.frontSpeed * randomValue;
            // position
            selectedPoint.x = playerController.transform.position.x - playerBehindLength;
            go.rb.position = selectedPoint;

            yield return new WaitForSeconds(spawnDelayTime);
            spawnDelayTime = Random.Range(minSpawnDelayTime, maxSpawnDelayTime);
        }
    }
}
