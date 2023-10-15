using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapObjectSpawner : Spawner
{
    public MapObject prefab;
    public float playerGapLength;
    public float minSpeed;
    public float maxSpeed;
    public float minSpawnDelayTime;
    public float maxSpawnDelayTime;

    public new void Start()
    {
        base.Start();

        spawnRangeValue = 3;
        spawnDelayTime = 3f;
        StartCoroutine(CreateObstacle());
    }

    public IEnumerator CreateObstacle()
    {
        while (!spawnStop)
        {
            yield return new WaitForSeconds(spawnDelayTime);
            spawnDelayTime = Random.Range(minSpawnDelayTime, maxSpawnDelayTime);

            if (GameManager.instance != null && GameManager.instance.isGameover)
            {
                break;
            }

            FindPlayerIndex();
            GetRandomSpawnIndex();
            MapObject go = ObjectPoolManager.instance.GetGo(prefab.name).GetComponent<MapObject>();
            // speed
            var randomValue = Random.Range(minSpeed, maxSpeed);
            go.speed = playerController.frontSpeed * randomValue;
            // position
            selectedPoint.x = playerController.transform.position.x + playerGapLength;
            selectedPoint.z = prefab.transform.position.z;
            go.transform.position = selectedPoint;
        }
    }
}
