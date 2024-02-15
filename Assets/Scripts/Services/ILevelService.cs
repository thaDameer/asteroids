using System;

public interface ILevelService
{
    public int _currentLevel { get; set; }
    public int CurrentLevel { get; set; }
    public AsteroidsInstaller.GameLevels.LevelData GetCurrentLevel();
    public Action<AsteroidsInstaller.GameLevels.LevelData> OnBuildLevelFromData { get; set; }

    public Action<int> OnNewLevelStarted { get; set; }
    void LevelCompleted();
    void ResetLevelProgression();
}