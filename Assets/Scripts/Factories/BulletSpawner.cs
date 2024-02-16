using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;


public class BulletSpawner : MonoBehaviour,IBulletSpawner
{
    [Header("Projectiles")]
    [SerializeField] private AssetReferenceT<Projectile> projectileReference;
    private Projectile shipProjectile;
    

    private ObjectPool<Projectile> pooledPlayerProjectiles;

    private void Awake()
    {
        Projectile.OnReturnToPool += TryReturnProjectileToPool;
    }

    private void OnDestroy()
    {
        Projectile.OnReturnToPool -= TryReturnProjectileToPool;
    }

    private void TryReturnProjectileToPool(Projectile projectile)
    {
        pooledPlayerProjectiles.Release(projectile);
    }
    private  void Start()
    {
        PreLoadProjectiles();
        pooledPlayerProjectiles = new ObjectPool<Projectile>(CreateShipProjectile, TakeFromPool, ReturnToPool);
    }
    
    async void PreLoadProjectiles()
    {
        var shipProjectileTask = PreloadGetComponent(projectileReference);
        await shipProjectileTask;
        if (shipProjectileTask.Result != null)
        {
            shipProjectile = shipProjectileTask.Result;
        }
    }
    private async Task<TComponent> PreloadGetComponent<TComponent>(AssetReferenceT<TComponent> assetReferenceT) where TComponent : Projectile
     { 
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(assetReferenceT);
        await handle.Task; 
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject loadedAsset = handle.Result;
            if (loadedAsset.TryGetComponent(out TComponent component))
            {
                return component;
            }   
        }
        else
        {
            Debug.LogError("Failed to load asset: " + handle.DebugName);
        }

        return null;
     }
    public void ShootPlayerBullet(ProjectileShootingData shootingData)
    {
        var projectile = pooledPlayerProjectiles.Get();
        projectile.transform.position = shootingData.Position;
        projectile.transform.rotation = shootingData.Rotation;
        projectile.Shoot(shootingData);
    }

    #region Object Pooling

    private Projectile CreateShipProjectile()
    {
        var clone = Instantiate(shipProjectile,transform);
        return clone;
    }
    
    private void TakeFromPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    private void ReturnToPool(Projectile projectile)
    {
        projectile.Activate(false);
        projectile.gameObject.SetActive(false);
    }


    #endregion

    
}

public interface IBulletSpawner
{
    public  void ShootPlayerBullet(ProjectileShootingData shootingData){}

    
    
}