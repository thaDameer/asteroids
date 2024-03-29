using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class LevelHandler : MonoBehaviour
{
    [Inject]
    private Camera _camera;
    private HashSet<MovementEntity> levelEntities = new HashSet<MovementEntity>();
    [Inject] private IEnemyService _enemyService;
    private PlayerShip currentShip;
    #region Services
    
    [Inject]
    private PlayerShip.Factory playerSpawnFactory;
    
 

    [Inject]
    public void Construct(IProjectileSpawner projectileSpawner)
    {
        projectileSpawner.OnProjectileCreated += AddProjectileToLevel;
    }
    [Inject]
    private ILevelService _levelService;

    
    #endregion

    [Inject] private IShipLifeCounter _shipLifeCounter;
    private void Awake()
    {
        _shipLifeCounter.OnLifeCountChanged += TrySpawnNewShip;
        _levelService.OnNewLevelStarted += StartNewLevel;
    }

    private void OnDestroy()
    {
        _shipLifeCounter.OnLifeCountChanged -= TrySpawnNewShip;
        _levelService.OnNewLevelStarted -= StartNewLevel;
    }

    private void TrySpawnNewShip(int counter)
    {
        if (counter > 0)
        {
            StartCoroutine(DelayedRespawn_CR());
            IEnumerator DelayedRespawn_CR()
            {
                yield return new WaitForSeconds(1.5f);
                SpawnPlayerShip();
            }
        }
    }
  
    private int currentPlayingLevel;
    private void StartNextLevel()
    { 

        _enemyService.Init(this);
        SetupLevel();   
    }

    private void StartNewLevel(int levelIndex)
    {
        StartNextLevel();
    }
    
   
    public void SetupLevel()
    {
        currentPlayingLevel = _levelService.CurrentLevel;
        bool isFirstLevel = currentPlayingLevel == 0;
        if (isFirstLevel)
        {
            currentPlayingLevel = -1;
            _levelService.ResetLevelProgression();
        }
        _enemyService.Setup(_levelService.GetCurrentLevel(),isFirstLevel);
        
        foreach (Transform transform in transform)
        {
            if (transform.TryGetComponent(out MovementEntity movingEntity))
            {
                levelEntities.Add(movingEntity);
            }
        }
        
        if(entitiesInBounds == null)
            entitiesInBounds = StartCoroutine(ProcessEntitiesInBounds());
    }

    private Coroutine entitiesInBounds;
    private void SpawnPlayerShip()
    {
        currentShip =playerSpawnFactory.Create();
        currentShip.transform.position =Vector3.zero;
        currentShip.transform.parent = transform;
        AddMovementEntity(currentShip);
    }

    #region  Level Boundaries

    private void AddProjectileToLevel(Projectile projectile)
    {
        AddMovementEntity(projectile);
    }  
    public void AddMovementEntity(MovementEntity movementEntity)
    {
        if(levelEntities.Contains(movementEntity))return;
        movementEntity.transform.parent = transform;
        levelEntities.Add(movementEntity);
    }
    IEnumerator ProcessEntitiesInBounds()
    {
        var wfs = new WaitForSeconds(0.2f);
        while (true)
        {
            levelEntities.RemoveWhere(x => x == null);
            foreach (var movingEntity in levelEntities)
            {
                if(movingEntity != null)
                    UpdateEntityWithinBounds(movingEntity);
            }

            yield return wfs;
        }
    }

    float screenPadding = 0.05f;
    void UpdateEntityWithinBounds(MovementEntity movementEntity)
    {
        Vector2 viewportPos = _camera.WorldToViewportPoint(movementEntity.transform.position);

        float newX = viewportPos.x;
        newX = TryFlipBounds(newX);
        float newY = viewportPos.y;
        newY = TryFlipBounds(newY);

        if (newX != viewportPos.x || newY != viewportPos.y)
        {
            movementEntity.OnBoundaryTeleportStart();
            var newPos = (Vector2)_camera.ViewportToWorldPoint(new Vector3(newX, newY));
            movementEntity.transform.position = newPos;
            movementEntity.OnBoundaryTeleportEnd();
        }

      
        float TryFlipBounds(float value)
        {
            if (value > 1 + screenPadding)
                return 0 - screenPadding;
            else if (value < 0 - screenPadding)
                return 1 + screenPadding;
            return value;
            
        }
    }
    #endregion
  


    public void LevelCompleted()
    {
        _levelService.LevelCompleted();
    }
}
