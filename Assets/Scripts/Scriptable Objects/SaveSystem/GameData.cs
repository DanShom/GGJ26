using System;
using System.Collections.Generic;
using UnityEngine;

public struct CompareScours : IComparer<scoreStruct>
{
    public int Compare(scoreStruct x, scoreStruct y)
    {
        if (x.score > y.score)
            return -1;
        else if (x.score < y.score)
            return 1;
        else
            return 0;
    }
}
[System.Serializable]
public struct scoreStruct
{
    public string characterId;
    public string name;
    public int score;
}

[System.Serializable]
public class GameData
{
    public bool newSave;
    public int highScore;
    public int moneyAmount;
    public int sailerId;
    public string playerName;
    public string currentId;
    public int language;

    public SerializableDictionary<string, long> longs;
    public SerializableDictionary<string, float> floats;
    public SerializableDictionary<string, int> ints;

    public GameData()
    {
        rest();
    }

    public void rest()
    {
        newSave = true;
        highScore = 0;
        moneyAmount = 0;
        playerName = "";
        sailerId = 0;
        currentId = "";
        language = (int)Language.English;

        longs    = new SerializableDictionary<string, long>();
        floats   = new SerializableDictionary<string, float>();
        ints = new SerializableDictionary<string, int>();
    }
}
