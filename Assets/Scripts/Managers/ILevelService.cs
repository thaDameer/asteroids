using System;

public interface ILevelService
{
    public int CurrentLevel { get; set; }
    public LevelData GetLevelData();
    public Action<LevelData> OnBuildLevelFromData { get; set; }
}