using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using Zenject;


public abstract class ShootingMovementEntity : MovementEntity
{
    
    [field: SerializeField]public ProjectileData ProjectileData { get; set; }

    private IBulletSpawner _bulletSpawner;

    [Inject]
    public void Construct(IBulletSpawner bulletSpawner)
    {
        this._bulletSpawner = bulletSpawner;
    }
    
    public virtual void Shoot()
    {
        ProjectileShootingData shootingData =
            new ProjectileShootingData(ProjectileData, transform, transform.up);

        _bulletSpawner.ShootPlayerBullet(shootingData);
    }
    public class Factory : PlaceholderFactory<ShootingMovementEntity> { }
}
[Serializable]
public struct ProjectileData
{
    public float ShootSpeed;
    public int HitDamage;
    public LayerMask TargetLayer;
}

public struct ProjectileShootingData
{
    public ProjectileData ProjectileData;
    public Vector2 Position;
    public Quaternion Rotation;
    public Vector2 Direction;
    public ProjectileShootingData(ProjectileData projectileData,Transform transform, Vector2 dir)
    {
        ProjectileData = projectileData;
        Position = transform.position;
        Rotation = transform.rotation;
        Direction = dir;
    }
}

