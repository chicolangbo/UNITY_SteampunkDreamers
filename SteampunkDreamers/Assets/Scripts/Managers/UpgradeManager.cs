using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
                    if(PlayDataManager.data.reinforceDatas["StartSpeedUpgrade"].id + 1 < 11)
                    {
                        prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["StartSpeedUpgrade"].id + 1).PRICE.ToString();
                    }
                    else
                    {
                        prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["StartSpeedUpgrade"].id).PRICE.ToString();
                    }
                    break;
                case 1:
                    if (PlayDataManager.data.reinforceDatas["RotateSpeedUpgrade"].id + 1 < 22)
                    {
                        prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["RotateSpeedUpgrade"].id + 1).PRICE.ToString();
                    }
                    else
                    {
                        prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["RotateSpeedUpgrade"].id).PRICE.ToString();
                    }
                    break;
                case 2:
                    if (PlayDataManager.data.reinforceDatas["CoinBonusUpgrade"].id + 1 < 33)
                    {
                        prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["CoinBonusUpgrade"].id + 1).PRICE.ToString();
                    }
                    else
                    {
                        prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["CoinBonusUpgrade"].id).PRICE.ToString();
                    }
                    break;
                case 3:
                    if (PlayDataManager.data.reinforceDatas["WeightLessUpgrade"].id + 1 < 44)
                    {
                        prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["WeightLessUpgrade"].id + 1).PRICE.ToString();
                    }
                    else
                    {
                        prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["WeightLessUpgrade"].id).PRICE.ToString();
                    }
                    break;
                case 4:
                    if (PlayDataManager.data.reinforceDatas["AeroBoostUpgrade"].id + 1 < 55)
                    {
                        prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["AeroBoostUpgrade"].id + 1).PRICE.ToString();
                    }
                    else
                    {
                        prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["AeroBoostUpgrade"].id).PRICE.ToString();
                    }
                    break;
                case 5:
                    var temp = table.GetData(PlayDataManager.data.reinforceDatas["MoreFuelUpgrade"].id + 1);
                    if (temp == null)
                    {
                        prices[i].text = table.GetData(PlayDataManager.data.reinforceDatas["MoreFuelUpgrade"].id).PRICE.ToString();
                    }
                    else
                    {
                        prices[i].text = temp.PRICE.ToString();
                    }
                    break;
            }
        }
    }

    private void Update()
    {
        // 돈 치트
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PlayDataManager.data.money += 100000;
        }    

        currentMoneyUI.text = PlayDataManager.data.money.ToString();
    }

    public void StartSpeedUpgrade()
    {
        var name = "StartSpeedUpgrade";
        if (CheckUpgrade(name))
        {
            SaveReinforceData(name, table.GetData(PlayDataManager.data.reinforceDatas[name].id + 1).PRICE);
            CheckAllUpgradeButtons();
        }
    }

    public void RotateSpeedUpgrade()
    {
        var name = "RotateSpeedUpgrade";
        if (CheckUpgrade(name))
        {
            SaveReinforceData(name, table.GetData(PlayDataManager.data.reinforceDatas[name].id + 1).PRICE);
            CheckAllUpgradeButtons();
        }
    }

    public void CoinBonusUpgrade()
    {
        var name = "CoinBonusUpgrade";
        if (CheckUpgrade(name))
        {
            SaveReinforceData(name, table.GetData(PlayDataManager.data.reinforceDatas[name].id + 1).PRICE);
            CheckAllUpgradeButtons();
        }
    }

    public void WeightLessUpgrade()
    {
        var name = "WeightLessUpgrade";
        if (CheckUpgrade(name))
        {
            SaveReinforceData(name, table.GetData(PlayDataManager.data.reinforceDatas[name].id + 1).PRICE);
            CheckAllUpgradeButtons();
        }
    }

    public void AeroBoostUpgrade()
    {
        var name = "AeroBoostUpgrade";
        if (CheckUpgrade(name))
        {
            SaveReinforceData(name, table.GetData(PlayDataManager.data.reinforceDatas[name].id + 1).PRICE);
            CheckAllUpgradeButtons();
        }
    }

    public void MoreFuelUpgrade()
    {
        var name = "MoreFuelUpgrade";
        if (CheckUpgrade(name))
        {
            SaveReinforceData(name, table.GetData(PlayDataManager.data.reinforceDatas[name].id + 1).PRICE);
            CheckAllUpgradeButtons();
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
                    upgradeButton[0].interactable = true;
                    prices[0].text = table.GetData(originData.reinforceDatas[name].id + 1).PRICE.ToString();
                    return true;
                }
                upgradeButton[0].interactable = false;
                break;

            case "RotateSpeedUpgrade":
                if (originData.money >= table.GetData(originData.reinforceDatas[name].id + 1).PRICE && originData.reinforceDatas[name].level < 10)
                {
                    upgradeButton[1].interactable = true;
                    prices[1].text = table.GetData(originData.reinforceDatas[name].id + 1).PRICE.ToString();
                    return true;
                }
                upgradeButton[1].interactable = false;
                break;

            case "CoinBonusUpgrade":
                if (originData.money >= table.GetData(originData.reinforceDatas[name].id + 1).PRICE && originData.reinforceDatas[name].level < 10)
                {
                    upgradeButton[2].interactable = true;
                    prices[2].text = table.GetData(originData.reinforceDatas[name].id + 1).PRICE.ToString();
                    return true;
                }
                upgradeButton[2].interactable = false;
                break;

            case "WeightLessUpgrade":
                if (originData.money >= table.GetData(originData.reinforceDatas[name].id + 1).PRICE && originData.reinforceDatas[name].level < 10)
                {
                    upgradeButton[3].interactable = true;
                    prices[3].text = table.GetData(originData.reinforceDatas[name].id + 1).PRICE.ToString();
                    return true;
                }
                upgradeButton[3].interactable = false;
                break;

            case "AeroBoostUpgrade":
                if (originData.money >= table.GetData(originData.reinforceDatas[name].id + 1).PRICE && originData.reinforceDatas[name].level < 10)
                {
                    upgradeButton[4].interactable = true;
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
                        upgradeButton[5].interactable = true;
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

    private void CheckAllUpgradeButtons()
    {
        CheckUpgrade("StartSpeedUpgrade");
        CheckUpgrade("RotateSpeedUpgrade");
        CheckUpgrade("CoinBonusUpgrade");
        CheckUpgrade("WeightLessUpgrade");
        CheckUpgrade("AeroBoostUpgrade");
        CheckUpgrade("MoreFuelUpgrade");
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
