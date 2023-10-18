//using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class SaveData
{
    public int Version { get; set; }

    public abstract SaveData VersionUp();
    
}

public class SaveDataV1 : SaveData
{
    public SaveDataV1()
    {
        Version = 1;
    }

    public int Gold { get; set; }
    public override SaveData VersionUp()
    {
        var data = new SaveDataV2();
        data.Gold = Gold;
        return data;
    }
}

public class SaveDataV2 : SaveData
{
    public SaveDataV2()
    {
        Version = 2;
    }

    public int Gold { get; set; }
    public string Name { get; set; } = "Unknown";
    public override SaveData VersionUp()
    {
        var data = new SaveDataV3();
        return data;
    }
}

public class SaveDataV3 : SaveData
{
    public SaveDataV3()
    {
        Version = 3;

    }

    //public List<CubeInfo> cubeList { get; set; } = new List<CubeInfo>();

    public override SaveData VersionUp()
    {
        return null;
    }
}