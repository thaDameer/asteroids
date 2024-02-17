
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class EnemyService : MonoBehaviour, IEnemyService
{
    
    public Action OnClearAllEnemies { get; set; }
    [Inject]
    private LargeMeteor.Factory largeMeteorFactory;
    [Inject]
    private MediumMeteor.Factory mediumMeteorFactory;
    [Inject]
    private SmallMeteor.Factory smallMeteorFactory;
    [Inject]
    private SpaceShip.Factory spaceShipFactory;
    
    
    
    private LevelHandler _levelHandler;

    [Inject] private IScoringService _scoringService;
    public List<IDestructibleOpponent> _destructibleOpponents { get; set; }
    private HashSet<IDestructibleOpponent> _destructibleOpponentsHash = new HashSet<IDestructibleOpponent>();
    
    public void Init(LevelHandler levelHandler)
    {
        this._levelHandler = levelHandler;
        _destructibleOpponents = new List<IDestructibleOpponent>();
    }



    public void Setup(AsteroidsInstaller.GameLevels.LevelData levelData,bool clearLevel = false)
    {
        if (clearLevel)
        {
            TryClearLevel();
        }
        if(_destructibleOpponents!=null)
            _destructibleOpponents.Clear();
        SpawnMeteors(largeMeteorFactory,levelData.meteorsAmount,levelData.spaceShipsAmount,Vector2.zero);
        spaceShipCounter = levelData.spaceShipsAmount;
    }    private void TryClearLevel()
    {
        OnClearAllEnemies?.Invoke();
        _destructibleOpponents.Clear();
    }
    
    
    public void AddDestructible(IDestructibleOpponent destructibleOpponent)
    {
        if (!_destructibleOpponents.Contains(destructibleOpponent))
        {
            _destructibleOpponents.Add(destructibleOpponent);
        }   
    }

    private void RemoveIDestructible(IDestructibleOpponent objectToDestroy)
    {
        _scoringService.UpdateGainedScore(objectToDestroy.Score);
        _destructibleOpponents.Remove(objectToDestroy);
    }

    private int spaceShipCounter = 0;
    public void RemoveDestructible(IDestructibleOpponent destructibleOpponent)
    {
        if (_destructibleOpponents.Contains(destructibleOpponent))
        {
            RemoveIDestructible(destructibleOpponent);
        }
        if(destructibleOpponent is Meteor)
            ProcessDestroyedMeteor(destructibleOpponent,destructibleOpponent.DestroyedPos);
        if(spaceShipCounter>0)
            TrySpawnSpaceShip(); 
        
        _destructibleOpponents.RemoveAll(x=> x == null);
        if(_destructibleOpponents.Count <= 0)
            _levelHandler.LevelCompleted();
    }

    private void TrySpawnSpaceShip()
    {
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            var spaceShip = spaceShipFactory.Create();
            spaceShip.Setup();
            AddDestructible(spaceShip);
            _levelHandler.AddMovementEntity(spaceShip);
            spaceShipCounter--;
        }
    }
    private void SpawnMeteors<T>(PlaceholderFactory<T> meteorFactory, int meteorCount,int spaceShipsCount,Vector2 newPos) where T : Meteor
    {
        for (int i = 0; i < meteorCount; i++)
        {
            var meteor = meteorFactory.Create();
            _levelHandler.AddMovementEntity(meteor);
            if (newPos == Vector2.zero)
                meteor.transform.position = GetRandomPositionWithinBounds(Screen.width, Screen.height);
            else meteor.transform.position = newPos;
            AddDestructible(meteor);
        }
    }
    private void ProcessDestroyedMeteor(IDestructibleOpponent destructibleOpponent,Vector2 destroyedPos)
    {
        switch (destructibleOpponent)
        {
            case SmallMeteor smallMeteor:
                break;
            case MediumMeteor mediumMeteor:
                SpawnMeteors(smallMeteorFactory,2,0,destroyedPos);
                break;
            case LargeMeteor largeMeteor:
                SpawnMeteors(mediumMeteorFactory,2,0, destroyedPos);
                break;
        }
        
    }
    Vector2 GetRandomPositionWithinBounds(float screenWidth, float screenHeight)
    {
        Vector2 randomPosition;
        do
        {
            randomPosition = new Vector2(Random.Range(0f, screenWidth), Random.Range(0f, screenHeight));
        }
        while (IsInsideCircle(randomPosition, Vector2.zero, 6));
        return randomPosition;
    }

    bool IsInsideCircle(Vector2 point, Vector2 circleCenter, float radius)
    {
        float distance = Vector2.Distance(point, circleCenter);
        return distance < radius;
    }


}

public interface IEnemyService
{
    public void Init(LevelHandler levelHandler);
    public void Setup(AsteroidsInstaller.GameLevels.LevelData levelData,bool clearLevel);
    List<IDestructibleOpponent> _destructibleOpponents { get; set; }
    public void AddDestructible(IDestructibleOpponent destructibleOpponent);
    public void RemoveDestructible(IDestructibleOpponent destructibleOpponent);
    public Action OnClearAllEnemies { get; set; }
}