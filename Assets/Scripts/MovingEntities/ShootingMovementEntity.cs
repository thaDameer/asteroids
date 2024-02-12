using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using Zenject;


public abstract class ShootingMovementEntity : MovementEntity
{
    
    [field: SerializeField]public BulletDataContainer BulletData { get; set; }
    [field: SerializeField]public AssetReference Projectile{ get; set; }

    //IBulletFactory
    public async virtual void Shoot(Vector2 dir)
    {
        var handle =BulletFactory.Instance.InstantiateProjectile(transform.position, transform.rotation);
        await handle.Task;

        if (handle.Result != null)
        {
            Projectile projectile = null;
            projectile = handle.Result.GetComponent<Projectile>();
            projectile.Shoot(BulletData);
        }
    }
}
[Serializable]
public class BulletDataContainer
{
    public float ShootSpeed;
    [FormerlySerializedAs("DamageOnHit")] public int HitDamage;
    public LayerMask TargetLayer;
}