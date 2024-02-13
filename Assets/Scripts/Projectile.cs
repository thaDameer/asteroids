using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [FormerlySerializedAs("setupData")] [FormerlySerializedAs("_dataContainer")] [SerializeField]private ProjectileData data;
    private float shootSpeed => data.ShootSpeed;
    private int HitDamage => data.HitDamage;
    private LayerMask targetLayer => data.TargetLayer;
    [SerializeField]private float radius = 0.2f;
    [SerializeField] private float distance = 0.2f;
    private Vector2 shootDir;
    public void Shoot(ProjectileShootingData projectileShootingData)
    {
        data = projectileShootingData.ProjectileData;
        shootDir = projectileShootingData.Direction;
    }
    public void Update()
    {
        if(shootDir == Vector2.zero) return;
        UpdateMovement();   
        ProcessRaycastHitDetection();
    }

    private void UpdateMovement()
    {
        transform.position += transform.up * shootSpeed * Time.deltaTime;
      
    }

    
    private void ProcessRaycastHitDetection()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius,transform.up,distance,targetLayer);
        
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out IDestructable result))
            {
                HitIDamageableTarget(result);
            }
        }
    }
    
    private void HitIDamageableTarget(IDestructable destructable)
    {
        destructable.TakeDamage(HitDamage);
        Destroy(gameObject);
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