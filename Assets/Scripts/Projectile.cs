using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [FormerlySerializedAs("inactive")] public bool active;
    public static Action<Projectile> OnReturnToPool;
    [FormerlySerializedAs("setupData")] [FormerlySerializedAs("_dataContainer")] [SerializeField]private ProjectileData data;
    private float shootSpeed => data.ShootSpeed;
    private int HitDamage => data.HitDamage;
    private LayerMask targetLayer => data.TargetLayer;
    [SerializeField]private float radius = 0.2f;
    [SerializeField] private float distance = 0.2f;
    private Vector2 shootDir;
    private float aliveCounter = 0;
    private float aliveDuration = 3;
    public void Shoot(ProjectileShootingData projectileShootingData)
    {
        Activate(true);
        aliveCounter = 0;
        data = projectileShootingData.ProjectileData;
        shootDir = projectileShootingData.Direction;
    }

    public void Activate(bool activate) => active = activate;
    public void Update()
    {
        if(shootDir == Vector2.zero || !active) return;
        aliveCounter += Time.deltaTime;
        if (aliveCounter >= aliveDuration)
        {
            OnReturnToPool?.Invoke(this);
        }
        ProcessRaycastHitDetection();
        UpdateMovement();   
    }

    private void UpdateMovement()
    {
        transform.position += transform.up * shootSpeed * Time.deltaTime;
      
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