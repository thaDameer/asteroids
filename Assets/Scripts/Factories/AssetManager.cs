using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetManager : MonoBehaviour,IAssetManager
{
    [SerializeField] private AssetReference projectileReference;
    
    
    private void Start()
    {
        LoadAsset<Projectile>(projectileReference);   
    }
    [ContextMenu("LOAD")]
    private void Load()
    {
        Debug.LogError(projectileReference);
        LoadAsset<Projectile>(projectileReference);
    }
    public async void LoadAsset<T>(AssetReference assetToLoad)
    {
        AsyncOperationHandle<GameObject> handle = assetToLoad.LoadAssetAsync<GameObject>();
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log(handle.Status);
        }
        else
        {
            Debug.LogError("Failed to load bullet prefab.");
        }
    }
}

public interface IAssetManager
{
    public async void LoadAsset<T>(AssetReference assetToLoad)
    {
    }
}
