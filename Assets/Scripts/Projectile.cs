using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BulletDataContainer _dataContainer;
    private float shootSpeed => _dataContainer.ShootSpeed;
    private int HitDamage => _dataContainer.HitDamage;
    private LayerMask targetLayer => _dataContainer.TargetLayer;
 
    public void Shoot(BulletDataContainer bulletData)
    {
        _dataContainer = bulletData;
    }
    public void Update()
    {
        if(_dataContainer==null) return;
        UpdateMovement();   
    }

    private void UpdateMovement()
    {
        transform.position += transform.up * shootSpeed * Time.deltaTime;
      
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == targetLayer)
        {
            if (other.gameObject.TryGetComponent(out IDamagable damagable))
            {
                HitIDamageableTarget(damagable);
            }
        }
    }

    private void HitIDamageableTarget(IDamagable damagable)
    {
        damagable.TakeDamage(HitDamage);
        Destroy(gameObject);
    }
}