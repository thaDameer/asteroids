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


    private List<Meteor> activeMeteors;
    private LevelHandler _levelHandler;
    
    public void Init(LevelHandler levelHandler)
    {
        this._levelHandler = levelHandler;
    }
    public void Setup(int levelCount)
    {

        SpawnMeteors(largeMeteorFactory,levelCount + 5,Vector2.zero);
    }

    private void SpawnMeteors(PlaceholderFactory<Meteor> meteorFactory, int amount,Vector2 newPos)
    {
        for (int i = 0; i < amount; i++)
        {
            var meteor = meteorFactory.Create();
            _levelHandler.AddMovementEntity(meteor);
            if (newPos == Vector2.zero)
                meteor.transform.position = GetRandomPositionWithinBounds(Screen.width, Screen.height);
            else meteor.transform.position = newPos;
            meteor.OnMeteorDestroyed += ProcessDestroyedMeteor;
        }
    }
    
    
    private void ProcessDestroyedMeteor(IMeteor meteor,Vector2 destroyedPos)
    {
       
        meteor.OnMeteorDestroyed -= ProcessDestroyedMeteor;
        
        switch (meteor.meteorSize)
        {
            case IMeteor.MeteorSize.Small:
                break;
            case IMeteor.MeteorSize.Medium:
                SpawnMeteors(smallMeteorFactory,2,destroyedPos);
                break;
            case IMeteor.MeteorSize.Large:
                SpawnMeteors(mediumMeteorFactory,2, destroyedPos);
                break;
            default:
                throw new ArgumentOutOfRangeException();
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
