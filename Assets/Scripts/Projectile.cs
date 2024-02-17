using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MovementEntity
{
    public bool active;
    public static Action<Projectile> OnReturnToPool;
    private ProjectileData data;
    private int HitDamage => data.HitDamage;
    private LayerMask targetLayer => data.TargetLayer;
    [SerializeField]private float radius = 0.2f;
    [SerializeField] private float distance = 0.2f;
    private float aliveCounter = 0;
    private float projectileDuration { get; set; }
    public override Vector3 MovementDirection { get; set; }
    
    [SerializeField] private GameObject playerProjectileSprite;
    [SerializeField] private GameObject enemyProjectile;
    
    public void Shoot(GlobalProjectileData globalProjectileData)
    {
        SetBulletGraphics(globalProjectileData.ProjectileData.TargetLayer);
        Activate(true);
        transform.localScale = Vector3.one;
        aliveCounter = 0;
        AccelerationSpeed = 0;
        data = globalProjectileData.ProjectileData;
        currentSpeed = globalProjectileData.ProjectileData.ShootSpeed;
        MaxSpeed = globalProjectileData.ProjectileData.ShootSpeed;
        projectileDuration = globalProjectileData.ProjectileDuration;
        
        MovementDirection = globalProjectileData.Direction;
    }

    private void SetBulletGraphics(LayerMask targetLayer)
    {
        bool isPlayerProjectile = LayerMask.GetMask("Player") != targetLayer;
        playerProjectileSprite.gameObject.SetActive(isPlayerProjectile);
        enemyProjectile.gameObject.SetActive(!isPlayerProjectile);
    }
    public void Activate(bool activate) => active = activate;
    public void Update()
    {
        
        if((Vector2)MovementDirection == Vector2.zero || !active) return;
        UpdateMovement(true);
        aliveCounter += Time.deltaTime;
        
        if (aliveCounter >= projectileDuration)
        {
            active = false;
            ReturnToPool();
            //OnReturnToPool?.Invoke(this);
        }
        ProcessRaycastHitDetection();
        UpdateMovement(true);   
    }

    private void ReturnToPool()
    {
        transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.OutQuint).OnComplete((() => OnReturnToPool?.Invoke(this)));
    }
   
    private void ProcessRaycastHitDetection()
    {
        var hit = Physics2D.OverlapCircle(transform.position, radius,targetLayer);
        
        if (hit != null)
        {
            if (hit.TryGetComponent(out IDestructable result))
            {
                HitIDamageableTarget(result);
            }
        }
    }
    private void HitIDamageableTarget(IDestructable destructable)
    {
        destructable.TakeDamage(HitDamage);
        OnReturnToPool?.Invoke(this);
    }

    private void OnDrawGizmos()
    {
        var color = Color.green;
        color.a = 0.3f;
        Gizmos.DrawRay(transform.position,transform.up*distance);
        var circleCastPos = transform.up * distance + transform.position;
        color = Color.magenta;
        color.a = 0.3f;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(circleCastPos,radius);
    }


 
}