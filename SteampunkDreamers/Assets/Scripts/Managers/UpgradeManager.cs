using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class CurrentReinforceData
{
    public int id = 0;
    public string name = "";
    public int level = 0;
}

public class UpgradeManager : MonoBehaviour
{
    public CurrentReinforceData[] currentReinforceDatas = new CurrentReinforceData[6];
    public GameObject[] window = new GameObject[6];

    private Button[] upgradeButton = new Button[6];
    private TextMeshProUGUI[] prices = new TextMeshProUGUI[6];
    private ReinforceTable table;
    private static UpgradeManager m_instance;
    private TextMeshProUGUI currentMoneyUI;

    public GameObject upgradeManager;

    public static UpgradeManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<UpgradeManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(instance);
        currentMoneyUI = GameObject.FindGameObjectWithTag("Money").GetComponent<TextMeshProUGUI>();
        table = DataTableMgr.GetTable<ReinforceTable>();
        for (int i = 0; i < window.Length; i++)
        {
            prices[i] = window[i].transform.GetChild(window[i].transform.childCount - 1).GetChild(window[i].transform.GetChild(window[i].transform.childCount - 1).childCount - 1).GetChild(0).GetComponent<TextMeshProUGUI>();
            upgradeButton[i] = window[i].transform.GetChild(window[i].transform.childCount - 1).GetChild(window[i].transform.GetChild(window[i].transform.childCount - 1).childCount - 3).GetComponent<Button>();
        }

        // currentReinforceDatas에 저장 데이터 씌우기
        // 초기 시작 코드
        for (int i = 0; i<currentReinforceDatas.Length; i++)
        {
            //var tempData = new CurrentReinforceData();
            currentReinforceDatas[i] = new CurrentReinforceData();
            currentReinforceDatas[i].level = 0;
            switch(i)
            {
                case 0: 
                    currentReinforceDatas[i].name = "StartSpeedUpgrade";
                    currentReinforceDatas[i].id = 0;
                    break;
                case 1: currentReinforceDatas[i].name = "RotateSpeedUpgrade";
                    currentReinforceDatas[i].id = 11;
                    break;
                case 2: currentReinforceDatas[i].name = "CoinBonusUpgrade";
                    currentReinforceDatas[i].id = 22;
                    break;
                case 3: currentReinforceDatas[i].name = "WeightLessUpgrade";
                    currentReinforceDatas[i].id = 33;
                    break;
                case 4: currentReinforceDatas[i].name = "AeroBoostUpgrade";
                    currentReinforceDatas[i].id = 44;
                    break;
                case 5: currentReinforceDatas[i].name = "MoreFuelUpgrade";
                    currentReinforceDatas[i].id = 55;
                    break;
            }
            prices[i].text = table.GetData(currentReinforceDatas[i].id + 1).PRICE.ToString();
        }

    }

    public void Start()
    {
        table = DataTableMgr.GetTable<ReinforceTable>();
        for (int i = 0; i < window.Length; i++)
        {
            prices[i] = window[i].transform.GetChild(window[i].transform.childCount - 1).GetChild(window[i].transform.GetChild(window[i].transform.childCount - 1).childCount - 1).GetChild(0).GetComponent<TextMeshProUGUI>();
            upgradeButton[i] = window[i].transform.GetChild(window[i].transform.childCount - 1).GetChild(window[i].transform.GetChild(window[i].transform.childCount - 1).childCount - 3).GetComponent<Button>();
        }

        for (int i = 1; i < table.reinforceDatas.Count; i++)
        {
            if (i % 10 == 1)
            {
                CheckUpgrade(table.GetData(i).NAME);
            }
        }
    }

    private void Update()
    {
        currentMoneyUI.text = GameManager.instance.money.ToString();
    }

    public void StartSpeedUpgrade()
    {
        if(CheckUpgrade("StartSpeedUpgrade"))
        {
            GameManager.instance.money -= table.GetData(currentReinforceDatas[0].id + 1).PRICE;
            currentReinforceDatas[0].level++;
            currentReinforceDatas[0].id++;
            CheckUpgrade("StartSpeedUpgrade");
        }
    }

    public void RotateSpeedUpgrade()
    {
        if (CheckUpgrade("RotateSpeedUpgrade"))
        {
            GameManager.instance.money -= table.GetData(currentReinforceDatas[1].id + 1).PRICE;
            currentReinforceDatas[1].level++;
            currentReinforceDatas[1].id++;
            CheckUpgrade("RotateSpeedUpgrade");
        }
    }

    public void CoinBonusUpgrade()
    {
        if (CheckUpgrade("CoinBonusUpgrade"))
        {
            GameManager.instance.money -= table.GetData(currentReinforceDatas[2].id + 1).PRICE;
            currentReinforceDatas[2].level++;
            currentReinforceDatas[2].id++;
            CheckUpgrade("CoinBonusUpgrade");
        }
    }

    public void WeightLessUpgrade()
    {
        if (CheckUpgrade("WeightLessUpgrade"))
        {
            GameManager.instance.money -= table.GetData(currentReinforceDatas[3].id + 1).PRICE;
            currentReinforceDatas[3].level++;
            currentReinforceDatas[3].id++;
            CheckUpgrade("WeightLessUpgrade");
        }
    }

    public void AeroBoostUpgrade()
    {
        if (CheckUpgrade("AeroBoostUpgrade"))
        {
            GameManager.instance.money -= table.GetData(currentReinforceDatas[4].id + 1).PRICE;
            currentReinforceDatas[4].level++;
            currentReinforceDatas[4].id++;
            CheckUpgrade("AeroBoostUpgrade");
        }
    }

    public void MoreFuelUpgrade()
    {
        if (CheckUpgrade("MoreFuelUpgrade"))
        {
            GameManager.instance.money -= table.GetData(currentReinforceDatas[5].id + 1).PRICE;
            currentReinforceDatas[5].level++;
            currentReinforceDatas[5].id++;
            CheckUpgrade("MoreFuelUpgrade");
        }
    }

    private bool CheckUpgrade(string name)
    {
        switch (name)
        {
            case "StartSpeedUpgrade":
                if (GameManager.instance.money >= table.GetData(currentReinforceDatas[0].id + 1).PRICE && currentReinforceDatas[0].level < 10)
                {
                    prices[0].text = table.GetData(currentReinforceDatas[0].id + 1).PRICE.ToString();
                    return true;
                }
                upgradeButton[0].interactable = false;
                break;

            case "RotateSpeedUpgrade":
                if (GameManager.instance.money >= table.GetData(currentReinforceDatas[1].id + 1).PRICE && currentReinforceDatas[1].level < 10)
                {
                    prices[1].text = table.GetData(currentReinforceDatas[1].id + 1).PRICE.ToString();
                    return true;
                }
                upgradeButton[1].interactable = false;
                break;

            case "CoinBonusUpgrade":
                if (GameManager.instance.money >= table.GetData(currentReinforceDatas[2].id + 1).PRICE && currentReinforceDatas[2].level < 10)
                {
                    prices[2].text = table.GetData(currentReinforceDatas[2].id + 1).PRICE.ToString();
                    return true;
                }
                upgradeButton[2].interactable = false;
                break;

            case "WeightLessUpgrade":
                if (GameManager.instance.money >= table.GetData(currentReinforceDatas[3].id + 1).PRICE && currentReinforceDatas[3].level < 10)
                {
                    prices[3].text = table.GetData(currentReinforceDatas[3].id + 1).PRICE.ToString();
                    return true;
                }
                upgradeButton[3].interactable = false;
                break;

            case "AeroBoostUpgrade":
                if (GameManager.instance.money >= table.GetData(currentReinforceDatas[4].id + 1).PRICE && currentReinforceDatas[4].level < 10)
                {
                    prices[4].text = table.GetData(currentReinforceDatas[4].id + 1).PRICE.ToString();
                    return true;
                }
                upgradeButton[4].interactable = false;
                break;

            case "MoreFuelUpgrade":
                if (GameManager.instance.money >= table.GetData(currentReinforceDatas[5].id + 1).PRICE && currentReinforceDatas[5].level < 10)
                {
                    prices[5].text = table.GetData(currentReinforceDatas[5].id + 1).PRICE.ToString();
                    return true;
                }
                upgradeButton[5].interactable = false;
                break;
        }
        return false;
    }
}
