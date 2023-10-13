using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float boardScaleX { get; private set; }
    public float boardScaleY;
    public float boardScaleZ;
    public GameObject player { get; private set; }

    public static GameManager instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<GameManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    private static GameManager m_instance; // �̱����� �Ҵ�� static ����

    public bool isGameover { get; private set; } // ���� ���� ����

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    public void SetBoardLength(float initialSpeed)
    {
        boardScaleX = 0.5f * initialSpeed * 6f + 20f;
        GameObject.FindWithTag("Board").transform.localScale = new Vector3(boardScaleX, boardScaleY, boardScaleZ);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ���� ���� ó��
    public void EndGame()
    {
        // ���� ���� ���¸� ������ ����
        isGameover = true;
        // ���� ���� UI�� Ȱ��ȭ
        // UIManager.instance.SetActiveGameoverUI(true);
    }
}
