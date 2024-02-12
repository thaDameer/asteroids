using System;

public interface ILevelManager
{
    public int CurrentLevel { get; set; }
    public LevelData GetLevelData();
    public Action<LevelData> OnBuildLevelFromData { get; set; }
}