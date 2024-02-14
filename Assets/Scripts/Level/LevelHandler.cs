using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelHandler : MonoBehaviour,IObserver
{
    private Camera camera;
    private List<MovementEntity> levelEntities = new List<MovementEntity>();
    [SerializeField] private MeteorHandler _meteorHandler;

    #region Services

    [Inject]
    private ILevelService _levelService;
    [Inject]
    private PlayerShip.Factory playerSpawnFactory;

    private IGameManager iGameManager;
    [Inject]
    public void Construct(IGameManager gameManager)
    {
        iGameManager = gameManager;
        gameManager.RegisterObserver(this);
    }
    
    #endregion

    public void Notify(GameState gameState)
    {
        if(gameState == GameState.Playing)
            StartLevel();
    }
    private void StartLevel()
    {
        _meteorHandler.Init(this);
        SetupLevel();   
    }
    private void Start()
    {
        camera = Camera.main;
        
    }

    private PlayerShip currentPlayer;
   
    public void SetupLevel()
    {
        _meteorHandler.Setup(_levelService.GetCurrentLevel());
        if (currentPlayer == null)
        {
            currentPlayer =playerSpawnFactory.Create();
            currentPlayer.transform.position =Vector3.zero;
            currentPlayer.transform.parent = transform;
        }
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
            
        }
    }



}
