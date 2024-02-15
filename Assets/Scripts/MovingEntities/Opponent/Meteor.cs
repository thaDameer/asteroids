using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Meteor : MovementEntity,IDestructable,IDestructibleOpponent
{
    [Inject] private IEnemyService _enemyService;
    public GameObject DestroyedGameObject => gameObject;
    public Vector2 DestroyedPos { get; set; }
    public int Score { get; set; }
    public override Vector3 MovementDirection { get; set; }
    private Vector2 rotateDir;
    [field: SerializeField]public SpriteRenderer SpriteRenderer { get; set; }
   public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }

    

    public void Setup<T>(MeteorData<T> meteorData)
    {
        SetupMeteor();
        targetLayer = meteorData.TargetLayer;
        Score = meteorData.Score;
        collisionRadius = meteorData.CollisionRadius;
        Setup(meteorData.MovementEntityData);
    }
    private void SetupMeteor()
    {
        SetRandomDirection();
        SetRandomRotationDirection();
        SetRandomSpeed();
    }

 
    private void SetRandomDirection()
    {
        MovementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void SetRandomSpeed()
    {
        currentSpeed *= Random.Range(0.8f, 1);
    }
    private void SetRandomRotationDirection()
    {
        rotateDir.x= Mathf.Clamp01(Random.Range(-1f, 1f));
    }
    private void Update()
    {
        UpdateRotation(rotateDir);
        UpdateMovement(true);
        UpdateCollisionDetection();
    }
    public void Died()
    {
        Destroy(gameObject);
    } 
    
    public void TakeDamage(int dmg)
    {
        CurrentHealth -= dmg;
        float flashDuration = 0.05f;
        if(CurrentHealth>0) return;
        DestroyedPos = transform.position;
        _enemyService.RemoveDestructible(this);
        SpriteHelperClass.Flash(SpriteRenderer,Color.black,flashDuration,2,(() =>
        {
            Died();
        }));
        
    }


    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float collisionRadius = 1;
    private void UpdateCollisionDetection()
    {
        var hit = Physics2D.OverlapCircle(transform.position, collisionRadius, targetLayer);
        
        if (hit != null)
        {
            if (hit.attachedRigidbody.TryGetComponent(out IDestructable result))
            {
                result.TakeDamage(1);
            }
        }
    }

    private void OnDrawGizmos()
    {
        var color = Color.green;
        color.a = 0.3f;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position,collisionRadius);
    }
   
}