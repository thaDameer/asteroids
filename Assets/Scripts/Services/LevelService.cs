using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class LevelService : ManagerBase,ILevelService
{

    public int _currentLevel { get; set; }

    public int CurrentLevel
    {
        get
        {
            return _currentLevel;
        }
        set
        {
            if (value != _currentLevel)
            {
                _currentLevel = value;
                OnNewLevelStarted?.Invoke(value);
            }
        }
    }
    [Inject] private AsteroidsInstaller.GameLevels _gameLevels;
    public AsteroidsInstaller.GameLevels.LevelData GetLevelData(int levelIndex)
    {
        return _gameLevels.Levels[_currentLevel % _gameLevels.Levels.Length];
    }
    public AsteroidsInstaller.GameLevels.LevelData GetCurrentLevel()
    {
        return GetLevelData(CurrentLevel);
    }

    public Action<AsteroidsInstaller.GameLevels.LevelData> OnBuildLevelFromData { get; set; }
    public Action<int> OnNewLevelStarted { get; set; }

    public void LevelCompleted()
    {
        CurrentLevel += 1;
    }

    public void ResetLevelProgression()
    {
       
    }


    private void Start() 
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        _currentLevel = -1;
    }

    
}

