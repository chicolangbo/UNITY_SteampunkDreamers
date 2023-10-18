using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject gameManager;

    public float boardScaleX { get; private set; }
    public float boardScaleY;
    public float boardScaleZ;
    public GameObject player { get; private set; }
    public int coinScore;
    public float basicScore;
    public float bonusScore;
    public static int money { get; private set; } = 0;

    private TextMeshProUGUI fps;

    public static GameManager instance = null;

    public bool isGameover { get; private set; } // ���� ���� ����

    private void Awake()
    {
        Init();

        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(instance);

        player = GameObject.FindGameObjectWithTag("Player");
        var inGameUI = GameObject.FindGameObjectWithTag("InGameUI");
        fps = inGameUI.transform.GetChild(inGameUI.transform.childCount - 2).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
        fps.text = "FPS : " + (1f / Time.deltaTime).ToString();
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

    public void UpdateMoney() // ���� ���� �� ȣ��
    {
        money += coinScore + (int)basicScore + (int)bonusScore;
    }

    public void SetPlayer(GameObject pl)
    {
        if (pl != null)
        {
            player = pl;
        }
        else
        {
            Debug.Log("player == null");
        }
    }

    // ���� ���� ó��
    public void EndGame()
    {
        // ���� ���� ���¸� ������ ����
        isGameover = true;
        // ���� ���� UI�� Ȱ��ȭ
        Time.timeScale = 0f;
        UIManager.instance.SetActiveGameoverUI(true);
        UIManager.instance.UpdateCoinScoreText(coinScore);
        UIManager.instance.UpdateBasicScoreText(basicScore);
        UIManager.instance.UpdateBonusScoreText(bonusScore);
        UpdateMoney();
    }

    public void Init()
    {
        isGameover = false;
        coinScore = 0;
        basicScore = 0f;
        bonusScore = 0f;
        // �� �ʱ�ȭ
    }

    public void MoneyChange(int diff)
    {
        money += diff;
    }
}
