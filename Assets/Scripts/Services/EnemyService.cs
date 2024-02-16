
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class EnemyService : MonoBehaviour, IEnemyService
{
    
    [Inject]
    private LargeMeteor.Factory largeMeteorFactory;
    [Inject]
    private MediumMeteor.Factory mediumMeteorFactory;
    [Inject]
    private SmallMeteor.Factory smallMeteorFactory;
    
    private LevelHandler _levelHandler;

    [Inject] private IScoringService _scoringService;
    
    public void Init(LevelHandler levelHandler)
    {
        this._levelHandler = levelHandler;
        _destructibleOpponents = new List<IDestructibleOpponent>();
    }


    private void TryClearLevel()
    {
        if (_destructibleOpponents.Count > 0)
        {
            foreach (var iDestructibleOpponent in _destructibleOpponents)
            {
                iDestructibleOpponent.ClearObject();
            }
            _destructibleOpponents.Clear();
        }
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
    }
    
    public List<IDestructibleOpponent> _destructibleOpponents { get; set; }
    private HashSet<IDestructibleOpponent> _destructibleOpponentsHash = new HashSet<IDestructibleOpponent>();

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
    public void RemoveDestructible(IDestructibleOpponent destructibleOpponent)
    {
        if (_destructibleOpponents.Contains(destructibleOpponent))
        {

            RemoveIDestructible(destructibleOpponent);
        }
        
       
        ProcessDestroyedMeteor(destructibleOpponent,destructibleOpponent.DestroyedPos);
        _destructibleOpponents.RemoveAll(x => !(x != null));
        var opponentsLeft = new List<IDestructibleOpponent>();
        foreach (var opponent in _destructibleOpponents)
        {
            if (opponent != null)
            {
                opponentsLeft.Add(opponent);
            }
        }
        
        _destructibleOpponents = opponentsLeft;
        if(_destructibleOpponents.Count <= 0)
            _levelHandler.LevelCompleted();
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
}