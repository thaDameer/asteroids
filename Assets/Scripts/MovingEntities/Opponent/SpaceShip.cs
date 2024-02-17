using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class SpaceShip : ShootingMovementEntity, IDestructibleOpponent,IDestructable
{
    public override Vector3 MovementDirection { get; set; }
    private List<Vector2> movePositions = new List<Vector2>();
    public Vector2 DestroyedPos { get; set; }
    public int Score { get; set; }
    private int randomDir;
    [Inject]private SpaceShipData _spaceShipData;

    [Inject] private Camera _camera;

    [Inject]private IEnemyService enemyService;
    [field: SerializeField]public SpriteRenderer SpriteRenderer { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    private void ClearAndDestroy()
    {
        enemyService.OnClearAllEnemies -= ClearAndDestroy;
        if(gameObject)
            Destroy(gameObject);
    }

    private float randomSize;
    private LayerMask targetLayer;
    public void Setup()
    {
        randomSize = Random.Range(_spaceShipData.minSize, _spaceShipData.maxSize);
        targetLayer = _spaceShipData.ProjectileData.TargetLayer;
        enemyService.OnClearAllEnemies += ClearAndDestroy;
        Setup(_spaceShipData.MovementEntityData);

        
        Score = _spaceShipData.Score;
        SetupRandomMovementPath();
        transform.localScale = Vector2.one * randomSize;
        transform.position = movePositions[0];
     

        shootInterval = _spaceShipData.ShootInterval;
        currentTarget = movePositions[1];
        MovementDirection = (currentTarget-(Vector2)transform.position).normalized;

    }

    private float shootInterval;
    private float shootCounter;
    private Vector2 currentTarget;

    private void SetupRandomMovementPath()
    {
        randomDir = Random.Range(0, 2);
        int points = 2;
        SetupRemainingMovementPoints(points);
        
    }

    private void Update()
    {
        float distToTarget = Vector2.Distance(transform.position, currentTarget);
        if (distToTarget < 0.1f)
        {
            Destroy(gameObject);
        }
        UpdateCollisionDetection();
        
        UpdateMovement(true);
    }

    private void ProcessShootingRoutine()
    {
        shootCounter += Time.deltaTime;
        if (shootCounter >= shootInterval)
        {
            shootCounter = 0;
            Shoot();
        }
    }

    protected override void Shoot()
    {
        base.Shoot();

        var target = Random.insideUnitCircle;
        var dir = (target - (Vector2)transform.position);
        GlobalProjectileData data =
            new GlobalProjectileData(ProjectileData, transform, dir);

        
        Setup(_spaceShipData.ProjectileData);
        _projectileSpawner.ShootPlayerBullet(data);
    }
    private void UpdateCollisionDetection()
    {
        var hit = Physics2D.OverlapCircle(transform.position, randomSize,targetLayer );
        
        if (hit != null)
        {
            if (hit.attachedRigidbody.TryGetComponent(out IDestructable result))
            {
                result.TakeDamage(1);
            }
        }
    }

    private void SetupRemainingMovementPoints(int points)
    {
        movePositions = new List<Vector2>();
        var startPos = GetLeftOrRightScreenPos(randomDir);
        var endPos = GetLeftOrRightScreenPos(randomDir == 0 ? 1 :0);
    
        movePositions.Add(startPos);
        movePositions.Add(endPos);
    }
    private Vector2 GetLeftOrRightScreenPos(int randomValue)
    {
        Vector3 spawnPosition = new Vector3();
        spawnPosition.y = Screen.height * Random.value;
        
        if (randomValue == 0) 
        {
            spawnPosition = _camera.ViewportToWorldPoint(new Vector3(0, Random.value, 0));
        }
        else
        {
            spawnPosition= _camera.ViewportToWorldPoint(new Vector3(1, Random.value, 0));
        }

        return spawnPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Color red = Color.red;
        red.a = .8f;
        Gizmos.color = red;
        foreach (var movePosition in movePositions)
        {
            Gizmos.DrawSphere(movePosition,0.5f);
        }
    }

    public class Factory : PlaceholderFactory<SpaceShip> { }

    
    public void TakeDamage(int dmg)
    {
        enemyService.RemoveDestructible(this);
        ClearAndDestroy();
    }
}
