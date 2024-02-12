using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

public class BulletFactory : MonoBehaviour,IBulletFactory
{
    [SerializeField] private AssetReferenceT<Projectile> projectileReference;
    public static BulletFactory Instance;
    private void Awake()
    {
        Instance = this;
    }

    public AsyncOperationHandle<GameObject> InstantiateProjectile(Vector2 position,Quaternion rotation, Transform parent = null)
    {
        AsyncOperationHandle<GameObject> handle = projectileReference.InstantiateAsync(position,rotation,parent);
        return handle;
    }
  
    
    public Projectile GetPooledBullet()
    {
        return null;
    }
}

public interface IBulletFactory 
{
    public Projectile GetPooledBullet();
}