using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirflowSpwaner : MonoBehaviour
{
    public Transform[] spawnPoints;
    private Transform prevPoint;
    public AirflowSystem airflowPrefab;
    private float spawnDelayTime = 2.5f;

    public void Start() // ���߿� �����ϱ�
    {
        StartCoroutine(CreateAirflow());
    }

    public IEnumerator CreateAirflow()
    {
        while (true)
        {
            if (GameManager.instance != null && GameManager.instance.isGameover)
            {
                break;
            }

            Transform randomPoint;
            do
            {
                randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            }
            while (prevPoint == randomPoint);
            prevPoint = randomPoint;

            var airflow = Instantiate(airflowPrefab, randomPoint.position, randomPoint.rotation);
            Debug.Log("����");
            airflow.Setup((AirflowType)Random.Range(0, 2));

            yield return new WaitForSeconds(spawnDelayTime);
        }
    }
}
