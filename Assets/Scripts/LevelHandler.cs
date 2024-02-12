using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] private Bounds bounds;
    private List<MovementEntity> levelEntities = new List<MovementEntity>();

    private bool levelSetup;

    private Camera camera;

    private ILevelManager levelManager;
    
    
    [Inject]
    public void Construct(ILevelManager levelManager)
    {
        this.levelManager = levelManager;
    }
    private void Start()
    {
        camera = Camera.main;
        Debug.Log(levelManager.CurrentLevel);
        Setup();
    }
    
   
    public void Setup()
    {
        foreach (Transform transform in transform)
        {
            if (transform.TryGetComponent(out MovementEntity movingEntity))
            {
                levelEntities.Add(movingEntity);
            }
        }

        levelSetup = true;
        StartCoroutine(ProcessEntitiesInBounds());
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
