using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AirflowSpwaner : Spawner
{
    public AirflowSystem airflowPrefab;
    float xScale;

    public void Start()
    {
        xScale = 3000f;
        spawnRangeValue = 3;
        spawnDelayTime = 2.5f;
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

            FindPlayerIndex();
            GetRandomSpawnIndex();

            var airflow = Instantiate(airflowPrefab, selectedPoint, Quaternion.identity);
            airflow.Setup((AirflowType)Random.Range(0, 2));
            var tempScale = new Vector3(xScale, yScale, 1f);
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
