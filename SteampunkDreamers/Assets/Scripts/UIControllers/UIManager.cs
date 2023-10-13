using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>(); // 씬에 배치돼있는 모든 게임오브젝트 순회하기 때문에 사용 X
            }

            return m_instance;
        }
    }

    private static UIManager m_instance; // 싱글톤이 할당될 변수

    public TextMeshProUGUI distance;
    public TextMeshProUGUI velocity;
    public TextMeshProUGUI altitude;
    public GameObject gameoverUI; // 게임 오버시 활성화할 UI 

    public void UpdateDistanceText(float d)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(d.ToString("F3"));
        sb.Append(" m");
        distance.text = sb.ToString();
    }

    public void UpdateVelocityText(float v)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(v.ToString("F3"));
        sb.Append(" m/s");
        velocity.text = sb.ToString();
    }

    public void UpdateAltitudeText(float a)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(a.ToString("F3"));
        sb.Append(" m");
        altitude.text = sb.ToString();
    }

    // 게임 오버 UI 활성화
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    // 게임 재시작
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
