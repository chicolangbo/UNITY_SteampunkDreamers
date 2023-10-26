using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using SaveDataVC = SaveDataV1;

public class PlayDataManager
{
    public static SaveDataVC data;
    private static int reinforceCount = 6;

    public static void Init()
    {
        data = SaveLoadSystem.Load("savefile.json") as SaveDataVC;
        if (data == null)
        {
            data = new SaveDataVC();
            FirstGameSet();
            data.isFirstGame = true;
            SaveLoadSystem.Save(data, "savefile.json");
        }
    }

    public static void Save()
    {
        SaveLoadSystem.Save(data, "savefile.json");
    }

    public static void Reset()
    {
        data = new SaveDataVC();
        Save();
    }

    private static void FirstGameSet()
    {
        for (int i = 0; i < reinforceCount; i++)
        {
            var tempReinforceData = new ReinforceData();
            string name;
            switch (i)
            {
                case 0:
                    name = "StartSpeedUpgrade";
                    tempReinforceData.id = 0;
                    tempReinforceData.level = 0;
                    tempReinforceData.name = name;
                    data.reinforceDatas.Add(name, tempReinforceData);
                    break;
                case 1:
                    name = "RotateSpeedUpgrade";
                    tempReinforceData.id = 11;
                    tempReinforceData.level = 0;
                    tempReinforceData.name = name;
                    data.reinforceDatas.Add(name, tempReinforceData);
                    break;
                case 2:
                    name = "CoinBonusUpgrade";
                    tempReinforceData.id = 22;
                    tempReinforceData.level = 0;
                    tempReinforceData.name = name;
                    data.reinforceDatas.Add(name, tempReinforceData);
                    break;
                case 3:
                    name = "WeightLessUpgrade";
                    tempReinforceData.id = 33;
                    tempReinforceData.level = 0;
                    tempReinforceData.name = name;
                    data.reinforceDatas.Add(name, tempReinforceData);
                    break;
                case 4:
                    name = "AeroBoostUpgrade";
                    tempReinforceData.id = 44;
                    tempReinforceData.level = 0;
                    tempReinforceData.name = name;
                    data.reinforceDatas.Add(name, tempReinforceData);
                    break;
                case 5:
                    name = "MoreFuelUpgrade";
                    tempReinforceData.id = 55;
                    tempReinforceData.level = 0;
                    tempReinforceData.name = name;
                    data.reinforceDatas.Add(name, tempReinforceData);
                    break;
            }
        }
    }
}