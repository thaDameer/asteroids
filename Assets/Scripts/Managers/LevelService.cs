using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class LevelService : ManagerBase,ILevelService,IObserver
{
    public int CurrentLevel { get; set; }
    [Inject] private AsteroidsInstaller.GameLevels _gameLevels;
    public AsteroidsInstaller.GameLevels.LevelData GetLevelData(int levelIndex)
    {
        return _gameLevels.Levels[levelIndex % _gameLevels.Levels.Length];
    }

    public AsteroidsInstaller.GameLevels.LevelData GetCurrentLevel()
    {
        return GetLevelData(CurrentLevel);
    }

    public Action<AsteroidsInstaller.GameLevels.LevelData> OnBuildLevelFromData { get; set; }

    private IGameManager _gameManager;
    [Inject]
    public void Construct(IGameManager gameManager)
    {
        _gameManager = gameManager;
        gameManager.RegisterObserver(this);
    }
    private void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        CurrentLevel = 0;
    }


    public void Notify(GameState gameState)
    {
        if (gameState == GameState.LevelCompleted)
        {
            CurrentLevel += 1;
            _gameManager.SetState(GameState.Playing);
        }
    }
}

