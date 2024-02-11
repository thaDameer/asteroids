using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;


public abstract class ShootingMovementEntity : MovementEntity
{
    
    [field: SerializeField]public BulletDataContainer BulletData { get; set; }
    [field: SerializeField]public Projectile Projectile{ get; set; }

    //IBulletFactory
    public virtual void Shoot(Vector2 dir)
    {
        var clone = Instantiate(Projectile, transform.position, transform.rotation);
        clone.Shoot(BulletData);
    }
}
[Serializable]
public class BulletDataContainer
{
    public float ShootSpeed;
    [FormerlySerializedAs("DamageOnHit")] public int HitDamage;
    public LayerMask TargetLayer;
}