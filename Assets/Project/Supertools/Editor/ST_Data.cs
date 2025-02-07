using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ST_Data 
{
    public int unitsIncrement;
    public int tensIncrement;
    public int hundredsIncrement;
    public bool incrementAndroidBundleVersion;

    public ST_Data()
    {
        unitsIncrement = 0;
        tensIncrement = 0;
        hundredsIncrement = 0;
    }
}
