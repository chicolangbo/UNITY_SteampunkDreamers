using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System;

public class ReinforceTable : DataTable
{
    public class ReinforceData
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public int LEVEL { get; set; }
        public int PRICE { get; set; }
        public float VALUE { get; set; }
        public string DESCRIPTION { get; set; }
    }

    public List<ReinforceData> reinforceDatas = new List<ReinforceData>();

    public ReinforceTable()
    {
        path = "ReinforceTable.csv";
        Load();
    }

    public override void Load()
    {
        var csvData = Resources.Load<TextAsset>(path); // 실행 중에 path에 있는 파일을 TextAsset으로 반환

        TextReader reader = new StringReader(csvData.text);

        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
        csvConfiguration.HasHeaderRecord = true; // 변수명이 똑같아야 함

        var csv = new CsvReader(reader, csvConfiguration);

        try
        {
            var records = csv.GetRecords<ReinforceData>();

            foreach(var record in records)
            {
                var temp = new ReinforceData();
                temp = record;
                reinforceDatas.Add(temp);
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError(ex.StackTrace);
        }
    }

    public ReinforceData GetData(int id)
    {
        if (id <= 0 || id > reinforceDatas.Count)
        {
            return null;
        }
        return reinforceDatas[id];
    }
}
