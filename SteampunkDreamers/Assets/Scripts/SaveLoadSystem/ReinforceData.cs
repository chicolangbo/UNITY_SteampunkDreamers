using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceData
{
    public int id;
    public string name;
    public int level;

    public ReinforceData() { }
    public ReinforceData(int id, string name, int level)
    {
        this.id = id;
        this.name = name;
        this.level = level;
    }
}