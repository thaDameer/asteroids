using System;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : ManagerBase,ILevelManager
{
    public int CurrentLevel { get; set; }

    private void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        CurrentLevel = 1;
    }

    private LevelData GetLevelData(int level)
    {
        return new LevelData
        {
            AmountOfMeteors = 1,
            AmountOfSpaceShips = 0
        };
    }
    public LevelData GetLevelData()
    {
        return GetLevelData(CurrentLevel);
    }
}

[Serializable]
public class LevelData
{
    public int AmountOfMeteors;
    public int AmountOfSpaceShips;

}
