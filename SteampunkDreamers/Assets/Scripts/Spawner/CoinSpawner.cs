using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : Spawner
{
    public GameObject[] coinPattern;
    public MapObject coinPrefab;
    public Transform[] coinPos;
    public float speed;
    public float minSpawnDelayTime;
    public float maxSpawnDelayTime;
    public float playerGapLength;

    private new void Start()
    {
        base.Start();

        spawnRangeValue = 3;
        spawnDelayTime = 3f;
        StartCoroutine(CreateCoins());
    }

    public IEnumerator CreateCoins()
    {
        while (!spawnStop)
        {
            Debug.Log("coin spawn!!!");
            yield return new WaitForSeconds(spawnDelayTime);
            spawnDelayTime = Random.Range(minSpawnDelayTime, maxSpawnDelayTime);

            if (GameManager.instance != null && GameManager.instance.isGameover)
            {
                break;
            }

            FindPlayerIndex();
            GetRandomSpawnIndex();

            var randomGroup = Random.Range(0, coinPattern.Length - 1);
            var childCount = coinPattern[randomGroup].transform.childCount;
            coinPos = new Transform[childCount];
            selectedPoint.x = playerController.transform.position.x + playerGapLength;
            coinPattern[randomGroup].transform.position = selectedPoint;

            for(int i = 0; i<childCount; ++i)
            {
                coinPos[i] = coinPattern[randomGroup].transform.GetChild(i);
                MapObject go = ObjectPoolManager.instance.GetGo(coinPrefab.name).GetComponent<MapObject>();
                // speed
                go.speed = speed;
                // position
                go.transform.position = coinPos[i].position;
            }
        }
    }
}
