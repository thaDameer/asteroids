using System;

public interface ILevelService
{
    public int CurrentLevel { get; set; }
    public AsteroidsInstaller.GameLevels.LevelData GetCurrentLevel();
    public Action<AsteroidsInstaller.GameLevels.LevelData> OnBuildLevelFromData { get; set; }
    
}