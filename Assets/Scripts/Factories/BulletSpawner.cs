using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class BulletSpawner : MonoBehaviour,IBulletSpawner
{
    [Header("Projectiles")]
    [SerializeField] private AssetReferenceT<Projectile> projectileReference;
    [Header("Player")]
    [SerializeField] private AssetReferenceT<PlayerShip> playerShipReference;

    public async void ShootPlayerBullet(ProjectileShootingData shootingData)
    {
        var handle = InstantiateProjectile(shootingData.Position,shootingData.Rotation,transform);
        await handle.Task;
        var projectile = handle.Result.GetComponent<Projectile>();
        projectile.Shoot(shootingData);
    }

    public AsyncOperationHandle<GameObject> InstantiateProjectile(Vector2 position,Quaternion rotation, Transform parent = null)
    {
        AsyncOperationHandle<GameObject> handle = projectileReference.InstantiateAsync(position,rotation,parent);
        return handle;
    }

    
}

public interface IBulletSpawner
{
    public async void ShootPlayerBullet(ProjectileShootingData shootingData){}
    public AsyncOperationHandle<GameObject> InstantiateProjectile(Vector2 pos, Quaternion rot, Transform parent = null);
    
    
}