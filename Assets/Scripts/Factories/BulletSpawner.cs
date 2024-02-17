using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;


public class ProjectileSpawner : MonoBehaviour,IProjectileSpawner
{
    [Header("Projectiles")]
    [SerializeField] private AssetReferenceT<Projectile> projectileReference;
    private Projectile shipProjectile;
    
    [Inject] private ProjectileSettings projectileSettings;
    public float ProjectileDurationTime => projectileSettings.projectileDuration;
    public Action<Projectile> OnProjectileCreated { get; set; }
    
    private ObjectPool<Projectile> pooledProjectiles;

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
        pooledProjectiles.Release(projectile);
    }
    private  void Start()
    {
        PreLoadProjectiles();
        pooledProjectiles = new ObjectPool<Projectile>(CreateProjectile, TakeFromPool, ReturnToPool);
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



    public void ShootPlayerBullet(GlobalProjectileData data)
    {
        var projectile = pooledProjectiles.Get();
        data.SetDuration(ProjectileDurationTime);
        projectile.transform.position = data.Position;
        projectile.transform.rotation = data.Rotation;
        projectile.Shoot(data);
    }

    

    #region Object Pooling

    private Projectile CreateProjectile()
    {
        var clone = Instantiate(shipProjectile,transform);
        OnProjectileCreated?.Invoke(clone);
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

public interface IProjectileSpawner
{
    public float ProjectileDurationTime { get; }
    public  void ShootPlayerBullet(GlobalProjectileData data){}

    public Action<Projectile> OnProjectileCreated { get; set; }

}