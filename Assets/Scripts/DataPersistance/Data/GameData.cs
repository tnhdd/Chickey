using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public Vector3 playerPosition;
    public List<int> unlockedLevels;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {
        // playerPosition = Vector3.zero;
        unlockedLevels = new List<int>();
    }

}