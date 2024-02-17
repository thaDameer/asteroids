using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using Zenject;


public abstract class ShootingMovementEntity : MovementEntity
{
    
    public ProjectileData ProjectileData { get; set; }

    protected IProjectileSpawner _projectileSpawner;

    protected void Setup(ProjectileData projectileData)
    {
        ProjectileData = projectileData;
    }

[Inject]
    public void Construct(IProjectileSpawner projectileSpawner)
    {
        this._projectileSpawner = projectileSpawner;
    }
    
    protected virtual void Shoot()
    {
        GlobalProjectileData data =
            new GlobalProjectileData(ProjectileData, transform, transform.up);

        _projectileSpawner.ShootPlayerBullet(data);
    }
    
}
[Serializable]
public struct ProjectileData
{
    public float ShootSpeed;
    public int HitDamage;
    public LayerMask TargetLayer;
}

public class GlobalProjectileData
{
    public ProjectileData ProjectileData;
    public Vector2 Position;
    public Quaternion Rotation;
    public Vector2 Direction { get; private set; }

    public float ProjectileDuration;
    public GlobalProjectileData(ProjectileData projectileData,Transform transform, Vector2 dir)
    {
        
        ProjectileData = projectileData;
        Position = transform.position;
        Rotation = transform.rotation;
        Direction = dir;
    }

    public void SetDuration(float duration) => ProjectileDuration = duration;
}

