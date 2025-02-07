using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ST_DataSaver
{
    static ST_Data sTData;
    static string fileName = "ST_Data.json";

    public static void CheckInitialized()
    {
        if (!File.Exists(Application.persistentDataPath + "/" + fileName))
        {
            Initialize();
        }
        else
        {
            if (sTData == null)
            {
                string str = File.ReadAllText(Application.persistentDataPath + "/" + fileName);
                sTData = JsonUtility.FromJson<ST_Data>(str);
            }
        }
    }
    public static void Initialize()
    {
        sTData = new ST_Data();
        SaveToFile();
    }
    public static void SaveToFile()
    {
        string saveDataJSON = JsonUtility.ToJson(sTData);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName, saveDataJSON);
    }

    public static ST_Data GetSTData()
    {
        CheckInitialized();
        return sTData;
    }

    public static void SetUnitsIncrement(int increment)
    {
        GetSTData().unitsIncrement = increment;
        SaveToFile();
    }
    public static void SetTensIncrement(int increment)
    {
        GetSTData().tensIncrement = increment;
        SaveToFile();
    }
    public static void SetHundredsIncrement(int increment)
    {
        GetSTData().hundredsIncrement = increment;
        SaveToFile();
    }

    public static void SetAndroidBundleIncrement(bool value)
    {
        GetSTData().incrementAndroidBundleVersion = value;
        SaveToFile();
    }
}
