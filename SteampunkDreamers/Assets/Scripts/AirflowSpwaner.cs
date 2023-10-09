using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirflowSpwaner : MonoBehaviour
{
    private Vector3[] spawnPoints = new Vector3[5];
    private Vector3 prevPoint;
    public AirflowSystem airflowPrefab;
    private float spawnDelayTime = 2.5f;
    public PlayerController playerController;
    public bool spawnStop = false;

    private float airflowYScale;

    public void Start()
    {
        playerController = GetComponent<PlayerController>();
        StartCoroutine(CreateAirflow());
        airflowYScale = playerController.gameObject.GetComponent<BoxCollider>().size.y * 30f;
    }

    public IEnumerator CreateAirflow()
    {
        while (!spawnStop)
        {
            if (GameManager.instance != null && GameManager.instance.isGameover)
            {
                break;
            }

            for(int i = 0; i< spawnPoints.Length; ++i)
            {
                var tempPos = new Vector3(-20f, playerController.transform.position.y + airflowYScale * (i - 2));
                spawnPoints[i] = tempPos;
            }

            Vector3 randomPoint;
            do
            {
                randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
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
