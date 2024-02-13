using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelHandler : MonoBehaviour
{
    private Camera camera;
    private List<MovementEntity> levelEntities = new List<MovementEntity>();
    private ILevelService _levelService;

    [SerializeField] private MeteorHandler _meteorHandler;
    [Inject]
    private PlayerShip.Factory playerSpawnFactory;
    
    [Inject]
    public void Construct(ILevelService levelService)
    {
        this._levelService = levelService;
    }
    


    private void Start()
    {
        camera = Camera.main;
        _meteorHandler.Init(this);
        SetupLevel();
    }
    
   
    public void SetupLevel()
    {
        _meteorHandler.Setup(_levelService.CurrentLevel);
        var clone =playerSpawnFactory.Create();
        clone.transform.position =Vector3.zero;
        
        clone.transform.parent = transform;
        foreach (Transform transform in transform)
        {
            if (transform.TryGetComponent(out MovementEntity movingEntity))
            {
                levelEntities.Add(movingEntity);
            }
        }
        
        StartCoroutine(ProcessEntitiesInBounds());
    }

    public void AddMovementEntity(MovementEntity movementEntity)
    {
        levelEntities.Add(movementEntity);
    }
    IEnumerator ProcessEntitiesInBounds()
    {
        var wfs = new WaitForSeconds(0.2f);
        while (true)
        {
            foreach (var movingEntity in levelEntities)
            {
                if(movingEntity)
                    UpdateEntityWithinBounds(movingEntity);
            }
            yield return wfs;
        }
    }

    float screenPadding = 0.05f;
    void UpdateEntityWithinBounds(MovementEntity movementEntity)
    {
        Vector2 viewportPos = camera.WorldToViewportPoint(movementEntity.transform.position);

        float newX = viewportPos.x;
        newX = TryFlipBounds(newX);
        float newY = viewportPos.y;
        newY = TryFlipBounds(newY);

        if (newX != viewportPos.x || newY != viewportPos.y)
        {
            var newPos = (Vector2)camera.ViewportToWorldPoint(new Vector3(newX, newY));
            movementEntity.transform.position = newPos;
        }

      
        float TryFlipBounds(float value)
        {
            if (value > 1 + screenPadding)
                return 0 - screenPadding;
            else if (value < 0 - screenPadding)
                return 1 + screenPadding;
            return value;
            //
            // value = value switch
            // {
            //     > 1 => 0+0.1f,
            //     < 0 => 1-0.1f,
            //     _ => value
            // };
            // return value;
        }
    }
}
