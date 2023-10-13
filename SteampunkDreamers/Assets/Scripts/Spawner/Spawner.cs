using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    public Vector3[] spawnPoints;
    public Vector3 prevPoint;
    public Vector3 selectedPoint;
    public float minHeight;
    public PlayerController playerController;
    public float yScale;
    public float planeMultipleHeight;
    public bool spawnStop = true;
    public int spawnPointCount;
    public int playerIndex;
    public float spawnDelayTime;
    public int spawnRangeValue;

    public void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        yScale = playerController.gameObject.GetComponent<BoxCollider>().size.y * planeMultipleHeight;
        spawnPointCount = 10000 / (int)yScale;
        spawnPoints = new Vector3[spawnPointCount];

        // 스폰 포인트 생성 
        for (int i = 0; i < spawnPoints.Length; ++i)
        {
            var tempPos = new Vector3(-20f, yScale / 2f + yScale * i - 0.5f + minHeight);
            spawnPoints[i] = tempPos;
        }
    }

    public void FindPlayerIndex()
    {
        float minValue = 1000f;
        for (int i = 0; i < spawnPoints.Length; ++i)
        {
            var intValue = Mathf.Abs(spawnPoints[i].y - playerController.transform.position.y);
            if (minValue > intValue)
            {
                minValue = intValue;
                playerIndex = i;
            }
        }
    }

    public void GetRandomSpawnIndex()
    {
        int randomValueStart = Mathf.Clamp(playerIndex - spawnRangeValue, 0, playerIndex - spawnRangeValue);
        int randomValueEnd = Mathf.Clamp(playerIndex + spawnRangeValue, playerIndex + spawnRangeValue, spawnPoints.Length - 1);

        do
        {
            selectedPoint = spawnPoints[Random.Range(randomValueStart, randomValueEnd)];
        }
        while (prevPoint == selectedPoint);
        prevPoint = selectedPoint;
    }
}
