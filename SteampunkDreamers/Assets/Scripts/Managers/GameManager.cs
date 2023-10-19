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
    public int coinBonus = 1;
    public float basicScore;
    public float bonusScore;
    private TextMeshProUGUI fps;

    public static GameManager instance = null;
    public ReinforceTable table { get; private set; }

    public bool isGameover { get; private set; } // 게임 오버 상태

    private void Awake()
    {
        PlayDataManager.Init();
        Init();

        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

        player = GameObject.FindGameObjectWithTag("Player");
        var inGameUI = GameObject.FindGameObjectWithTag("InGameUI");
        fps = inGameUI.transform.GetChild(inGameUI.transform.childCount - 2).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        table = new ReinforceTable();
        var value = (int)table.GetData(PlayDataManager.data.reinforceDatas["CoinBonusUpgrade"].id).VALUE;
        if (value > 0)
        {
            coinBonus = value;
        }
        Debug.Log("코인 보너스값" + coinBonus);
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

    public void UpdateMoney() // 점수 집계 시 호출
    {
        PlayDataManager.data.money += coinScore * coinBonus + (int)basicScore + (int)bonusScore;
        PlayDataManager.Save();
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

    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        isGameover = true;
        // 게임 오버 UI를 활성화
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
    }
}
