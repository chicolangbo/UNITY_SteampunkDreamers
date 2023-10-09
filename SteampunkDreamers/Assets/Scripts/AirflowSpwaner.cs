using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirflowSpwaner : MonoBehaviour
{
    private Vector3[] spawnPoints;
    private Vector3 prevPoint;
    public AirflowSystem airflowPrefab;
    private float spawnDelayTime = 2.5f;
    public PlayerController playerController;
    public bool spawnStop = false;
    private int spawnWave = 0;

    private float airflowYScale;

    public void Start()
    {
        playerController = GetComponent<PlayerController>();
        airflowYScale = playerController.gameObject.GetComponent<BoxCollider>().size.y * 10f;
        int airflowCount = 10000 / (int)airflowYScale;
        spawnPoints = new Vector3[airflowCount];

        // 스폰 포인트 생성 
        for (int i = 0; i < spawnPoints.Length; ++i)
        {
            var tempPos = new Vector3(-20f, airflowYScale / 2f + airflowYScale * i - 0.5f);
            spawnPoints[i] = tempPos;
        }

        StartCoroutine(CreateAirflow());
    }

    public IEnumerator CreateAirflow()
    {
        while (!spawnStop)
        {
            if (GameManager.instance != null && GameManager.instance.isGameover)
            {
                break;
            }

            if((int)playerController.transform.position.x/3000 != spawnWave)
            {
                spawnWave = (int)playerController.transform.position.x / 3000;
                for (int i = 0; i < spawnPoints.Length; ++i)
                {
                    spawnPoints[i].x = 3000f * spawnWave - 20f;
                }
            }

            // 스폰 포인트 순회하면서 플레이어가 속한 기류 인덱스 찾기
            int standardIndex = 0;
            float minValue = 1000f;
            for(int i = 0; i < spawnPoints.Length; ++i)
            {
                var intValue = Mathf.Abs(spawnPoints[i].y - playerController.transform.position.y);
                if(minValue > intValue)
                {
                    minValue = intValue;
                    standardIndex = i;
                }
            }

            Vector3 randomPoint;
            do
            {
                randomPoint = spawnPoints[Random.Range((standardIndex - 2 < 0)? 0 : standardIndex - 2, (standardIndex + 2 > spawnPoints.Length - 1)? spawnPoints.Length -1 : standardIndex + 2)];
            }
            while (prevPoint == randomPoint);
            prevPoint = randomPoint;

            var airflow = Instantiate(airflowPrefab, randomPoint, Quaternion.identity);
            airflow.Setup((AirflowType)Random.Range(0, 2));
            var tempScale = new Vector3(3000f, airflowYScale, 1f);
            airflow.transform.localScale = tempScale;
            airflow.onDisappear += () =>
            {
                if(playerController.airflows.Contains(airflow))
                {
                    playerController.airflows.Remove(airflow);
                }
                Destroy(airflow.gameObject);
            };

            yield return new WaitForSeconds(spawnDelayTime);
        }
    }
}
