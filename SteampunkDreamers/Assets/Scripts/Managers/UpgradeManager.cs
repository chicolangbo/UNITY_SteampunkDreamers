using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using SaveDataVC = SaveDataV1; // 교체

public class CurrentReinforceData
{
    public int id = 0;
    public string name = "";
    public int level = 0;
}

public class UpgradeManager : MonoBehaviour
{
    public GameObject[] window = new GameObject[6];
    private Button[] upgradeButton = new Button[6];
    private TextMeshProUGUI[] prices = new TextMeshProUGUI[6];
    private ReinforceTable table;
    private TextMeshProUGUI currentMoneyUI;

    private void Awake()
    {
        Time.timeScale = 1.0f;

        // 저장 데이터 씌우기?
        PlayDataManager.Init();

        Init();
        for(int i = 0; i < prices.Length; i ++)
        {
            switch(i)
            {
                case 0:
                    prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["StartSpeedUpgrade"].id + 1).PRICE.ToString();
                    break;
                case 1:
                    prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["RotateSpeedUpgrade"].id + 1).PRICE.ToString();
                    break;
                case 2:
                    prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["CoinBonusUpgrade"].id + 1).PRICE.ToString();
                    break;
                case 3:
                    prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["WeightLessUpgrade"].id + 1).PRICE.ToString();
                    break;
                case 4:
                    prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["AeroBoostUpgrade"].id + 1).PRICE.ToString();
                    break;
                case 5:
                    prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["MoreFuelUpgrade"].id + 1).PRICE.ToString();
                    break;
            }
        }
    }

    private void Update()
    {
        currentMoneyUI.text = GameManager.money.ToString();

        // 저장 테스트
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    // save data 만들기
        //    ReinforceData reinforceData = new ReinforceData();
        //    reinforceData.name = "aaa";
        //    reinforceData.id = 1;
        //    reinforceData.level = 1;
        //    SaveDataV1 saveData = new SaveDataV1();
        //    saveData.reinforceDatas.Add(reinforceData);
        //    PlayDataManager.data = saveData;
        //    PlayDataManager.Save();
        //}
    }

    public void StartSpeedUpgrade()
    {
        var name = "StartSpeedUpgrade";
        if (CheckUpgrade(name))
        {
            SaveReinforceData(name, table.GetData(PlayDataManager.data.reinforceDatas[name].id + 1).PRICE);
            CheckUpgrade(name); // 버튼 막기
        }
    }

    public void RotateSpeedUpgrade()
    {
        var name = "RotateSpeedUpgrade";
        if (CheckUpgrade(name))
        {
            SaveReinforceData(name, table.GetData(PlayDataManager.data.reinforceDatas[name].id + 1).PRICE);
            CheckUpgrade(name);
        }
    }

    public void CoinBonusUpgrade()
    {
        var name = "CoinBonusUpgrade";
        if (CheckUpgrade(name))
        {
            SaveReinforceData(name, table.GetData(PlayDataManager.data.reinforceDatas[name].id + 1).PRICE);
            CheckUpgrade(name);
        }
    }

    public void WeightLessUpgrade()
    {
        var name = "WeightLessUpgrade";
        if (CheckUpgrade(name))
        {
            SaveReinforceData(name, table.GetData(PlayDataManager.data.reinforceDatas[name].id + 1).PRICE);
            CheckUpgrade(name);
        }
    }

    public void AeroBoostUpgrade()
    {
        var name = "AeroBoostUpgrade";
        if (CheckUpgrade(name))
        {
            SaveReinforceData(name, table.GetData(PlayDataManager.data.reinforceDatas[name].id + 1).PRICE);
            CheckUpgrade(name);
        }
    }

    public void MoreFuelUpgrade()
    {
        var name = "MoreFuelUpgrade";
        if (CheckUpgrade(name))
        {
            SaveReinforceData(name, table.GetData(PlayDataManager.data.reinforceDatas[name].id + 1).PRICE);
            CheckUpgrade(name);
        }
    }

    private bool CheckUpgrade(string name)
    {
        var originData = PlayDataManager.data;

        switch (name)
        {
            case "StartSpeedUpgrade":
                if (originData.money >= table.GetData(originData.reinforceDatas[name].id + 1).PRICE && originData.reinforceDatas[name].level < 10)
                {
                    prices[0].text = table.GetData(originData.reinforceDatas[name].id + 1).PRICE.ToString();
                    return true;
                }
                upgradeButton[0].interactable = false;
                break;

            case "RotateSpeedUpgrade":
                if (originData.money >= table.GetData(originData.reinforceDatas[name].id + 1).PRICE && originData.reinforceDatas[name].level < 10)
                {
                    prices[1].text = table.GetData(originData.reinforceDatas[name].id + 1).PRICE.ToString();
                    return true;
                }
                upgradeButton[1].interactable = false;
                break;

            case "CoinBonusUpgrade":
                if (originData.money >= table.GetData(originData.reinforceDatas[name].id + 1).PRICE && originData.reinforceDatas[name].level < 10)
                {
                    prices[2].text = table.GetData(originData.reinforceDatas[name].id + 1).PRICE.ToString();
                    return true;
                }
                upgradeButton[2].interactable = false;
                break;

            case "WeightLessUpgrade":
                if (originData.money >= table.GetData(originData.reinforceDatas[name].id + 1).PRICE && originData.reinforceDatas[name].level < 10)
                {
                    prices[3].text = table.GetData(originData.reinforceDatas[name].id + 1).PRICE.ToString();
                    return true;
                }
                upgradeButton[3].interactable = false;
                break;

            case "AeroBoostUpgrade":
                if (originData.money >= table.GetData(originData.reinforceDatas[name].id + 1).PRICE && originData.reinforceDatas[name].level < 10)
                {
                    prices[4].text = table.GetData(originData.reinforceDatas[name].id + 1).PRICE.ToString();
                    return true;
                }
                upgradeButton[4].interactable = false;
                break;

            case "MoreFuelUpgrade":
                if(originData.reinforceDatas[name].id + 1 < table.reinforceDatas.Count)
                {
                    if (originData.money >= table.GetData(originData.reinforceDatas[name].id + 1).PRICE && originData.reinforceDatas[name].level < 10)
                    {
                        prices[5].text = table.GetData(originData.reinforceDatas[name].id + 1).PRICE.ToString();
                        return true;
                    }
                    upgradeButton[5].interactable = false;
                }
                else
                {
                    upgradeButton[5].interactable = false;
                }
                break;
        }
        return false;
    }

    private void Init()
    {
        // 컴포넌트 연결
        currentMoneyUI = GameObject.FindGameObjectWithTag("Money").GetComponent<TextMeshProUGUI>();
        table = DataTableMgr.GetTable<ReinforceTable>();
        for (int i = 0; i < window.Length; i++)
        {
            prices[i] = window[i].transform.GetChild(window[i].transform.childCount - 1).GetChild(window[i].transform.GetChild(window[i].transform.childCount - 1).childCount - 1).GetChild(0).GetComponent<TextMeshProUGUI>();
            upgradeButton[i] = window[i].transform.GetChild(window[i].transform.childCount - 1).GetChild(window[i].transform.GetChild(window[i].transform.childCount - 1).childCount - 3).GetComponent<Button>();
        }
        // 버튼 제한
        for (int i = 1; i < table.reinforceDatas.Count; i++)
        {
            if (i % 10 == 1)
            {
                CheckUpgrade(table.GetData(i).NAME);
            }
        }
    }

    private void SaveReinforceData(string name, int price)
    {
        // 기존 정보에 ++
        var originData = PlayDataManager.data;
        if(originData.reinforceDatas.ContainsKey(name))
        {
            originData.reinforceDatas[name].level++;
            originData.reinforceDatas[name].id++;
        }
        else
        {
            ReinforceData reinforceData = new ReinforceData();
            reinforceData.name = name;
            reinforceData.id++;
            reinforceData.level++;
            originData.reinforceDatas.Add(name, reinforceData);
        }
        originData.money -= price;
        PlayDataManager.Save();
    }
}
