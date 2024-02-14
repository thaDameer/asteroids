using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class MeteorHandler : MonoBehaviour
{
    [Inject]
    private Meteor.LargeMeteor largeMeteorFactory;
    [Inject]
    private Meteor.MediumMeteor mediumMeteorFactory;
    [Inject]
    private Meteor.SmallMeteor smallMeteorFactory;


    private List<IMeteor> activeMeteors = new List<IMeteor>();
    private LevelHandler _levelHandler;

    [Inject] private IGameManager iGameManager;
    
    public void Init(LevelHandler levelHandler)
    {
        this._levelHandler = levelHandler;
    }
    public void Setup(AsteroidsInstaller.GameLevels.LevelData levelData)
    {

        activeMeteors.Clear();
        SpawnMeteors(largeMeteorFactory,levelData.meteorsAmount,levelData.spaceShipsAmount,Vector2.zero);
    }

    private void SpawnMeteors(PlaceholderFactory<Meteor> meteorFactory, int meteorCount,int spaceShipsCount,Vector2 newPos)
    {
        for (int i = 0; i < meteorCount; i++)
        {
            var meteor = meteorFactory.Create();
            _levelHandler.AddMovementEntity(meteor);
            if (newPos == Vector2.zero)
                meteor.transform.position = GetRandomPositionWithinBounds(Screen.width, Screen.height);
            else meteor.transform.position = newPos;
            activeMeteors.Add((IMeteor)meteor);
            meteor.OnMeteorDestroyed += ProcessDestroyedMeteor;
        }
    }
    
    
    private void ProcessDestroyedMeteor(IMeteor meteor,Vector2 destroyedPos)
    {
        if(activeMeteors.Count<=0)return;
        
        meteor.OnMeteorDestroyed -= ProcessDestroyedMeteor;
        if (activeMeteors.Contains(meteor))
            activeMeteors.Remove(meteor);
        switch (meteor.meteorSize)
        {
            case IMeteor.MeteorSize.Small:
                break;
            case IMeteor.MeteorSize.Medium:
                SpawnMeteors(smallMeteorFactory,2,0,destroyedPos);
                break;
            case IMeteor.MeteorSize.Large:
                SpawnMeteors(mediumMeteorFactory,2,0, destroyedPos);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        if (activeMeteors.Count <= 0)
        {
            iGameManager.SetState(GameState.LevelCompleted);
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
