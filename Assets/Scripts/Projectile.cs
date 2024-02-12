using System;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField]private BulletDataContainer _dataContainer;
    private float shootSpeed => _dataContainer.ShootSpeed;
    private int HitDamage => _dataContainer.HitDamage;
    private LayerMask targetLayer => _dataContainer.TargetLayer;

    [SerializeField]private float radius = 0.2f;
    [SerializeField] private float distance = 0.2f;
    public void Shoot(BulletDataContainer bulletData)
    {
        _dataContainer = bulletData;
    }
    public void Update()
    {
        if(_dataContainer==null) return;
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