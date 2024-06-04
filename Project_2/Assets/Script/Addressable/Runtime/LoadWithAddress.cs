using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Exceptions;
using UnityEngine.ResourceManagement.ResourceLocations;

public class LoadWithAddress : MonoBehaviour
{
    // Start the load operation on start
    void LoadAssetAsync1()
    {
        // Assign in Editor or in code
        string address = "DDDD/DDD/DSDS.prefab";
        // Retain handle to release asset and operation
        // 异步加载单个资源，可以使用 address、Label
        AsyncOperationHandle <GameObject> handle = Addressables.LoadAssetAsync<GameObject>(address);
        handle.Completed += Handle_Completed;
    }

    // Instantiate the loaded prefab on complete
    private void Handle_Completed(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject obj = handle.Result;
            Instantiate(obj, transform);
        }
        else
        {
            Debug.LogError($"Asset for {handle.DebugName} failed to load.");
            DownloadError(handle);
            Addressables.Release(handle);
        }
    }

    async void LoadAssetAsync2()
    {
        // Assign in Editor or in code
        string address = "DDDD/DDD/DSDS.prefab";
        // Retain handle to release asset and operation
        // 异步加载单个资源，可以使用 address、Label
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(address);
        // 使用 await 在同一帧等待
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject obj = handle.Result;
            Instantiate(obj, transform);
        }
        else
        {
            DownloadError(handle);
            Addressables.Release(handle);
        }
    }

    private IEnumerator LoadAssetAsync3()
    {
        // Assign in Editor or in code
        string address = "DDDD/DDD/DSDS.prefab";
        // Retain handle to release asset and operation
        // 异步加载单个资源，可以使用 address、Label
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(address);
        if (!handle.IsDone)
        {
            yield return handle;
        }
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject obj = handle.Result;
            Instantiate(obj, transform);
        }
        else
        {
            DownloadError(handle);
            Addressables.Release(handle);
        }
    }

    // 加载失败，出现异常时打印错误信息
    private void DownloadError(AsyncOperationHandle handle)
    {
        if (handle.Status == AsyncOperationStatus.Failed)
        {
            return;
        }

        System.Exception ex = handle.OperationException;
        while (ex != null)
        {
            RemoteProviderException remoteException = ex as RemoteProviderException;
            if (remoteException != null)
            {
                string error = remoteException.WebRequestResult.Error;
                break;
            }
            ex = ex.InnerException;
        }
    }

    private IEnumerator LoadWithLabel()
    {
        // 使用 key 加载多个资源
        List<string> keys = new List<string>() { "characters", "animals" };
        AsyncOperationHandle<IList<GameObject>> loadHandle;
        float x = 0, z = 0;
        loadHandle = Addressables.LoadAssetsAsync<GameObject>(
            keys, // Either a single key or a List of keys 
            addressable =>
            {
                //Gets called for every loaded asset
                if (addressable != null)
                {
                    Vector3 position = new Vector3(x++ * 2.0f, 0, z * 2.0f);
                    Instantiate<GameObject>(addressable, position, Quaternion.identity, transform);
                    if (x > 9)
                    {
                        x = 0;
                        z++;
                    }
                }
            }, Addressables.MergeMode.Union, // How to combine multiple labels 
            false); // Whether to fail if any asset fails to lo

        yield return loadHandle;
    }
    
    private IEnumerator LoadWithLabel2()
    {
        // 使用 Label 加载每个资源的 IResourceLocation，然后通过 IResourceLocation 加载每一个资源
        Dictionary<string, GameObject> _preloadedObjects = new Dictionary<string, GameObject>();
        //find all the locations with label "SpaceHazards"
        var loadResourceLocationsHandle = Addressables.LoadResourceLocationsAsync("SpaceHazards", typeof(GameObject));

        if (!loadResourceLocationsHandle.IsDone)
        { 
            yield return loadResourceLocationsHandle;
        }

        //start each location loading
        List<AsyncOperationHandle> opList = new List<AsyncOperationHandle>();
        foreach (IResourceLocation location in loadResourceLocationsHandle.Result)
        {
            AsyncOperationHandle<GameObject> loadAssetHandle = Addressables.LoadAssetAsync<GameObject>(location);
            loadAssetHandle.Completed += obj => { _preloadedObjects.Add(location.PrimaryKey, obj.Result); };
            opList.Add(loadAssetHandle);
        }

        //create a GroupOperation to wait on all the above loads at once. 
        var groupOp = Addressables.ResourceManager.CreateGenericGroupOperation(opList);

        if (!groupOp.IsDone)
            yield return groupOp;

        Addressables.Release(loadResourceLocationsHandle);

        //take a gander at our results.
        foreach (var item in _preloadedObjects)
        {
            Debug.Log(item.Key + " - " + item.Value.name);
        }
    }

    private void LoadWithLabel3()
    {
        // 使用 Label 加载每个资源的 IResourceLocation，然后通过 IResourceLocation 加载每一个资源
        Dictionary<string, GameObject> _preloadedObjects = new Dictionary<string, GameObject>();
        //find all the locations with label "SpaceHazards"
        var loadResourceLocationsHandle = Addressables.LoadResourceLocationsAsync("SpaceHazards", typeof(GameObject));
        loadResourceLocationsHandle.Completed += OnLoadComplete3;
    }

    private void OnLoadComplete3(AsyncOperationHandle<IList<IResourceLocation>> handle)
    {
        foreach (IResourceLocation location in handle.Result)
        {
            AsyncOperationHandle<GameObject> loadAssetHandle = Addressables.LoadAssetAsync<GameObject>(location);
            loadAssetHandle.Completed += obj => {
                GameObject go = obj.Result;
                Instantiate(go);
            };
        }
    }

    public async void LoadWithLabel4()
    {
        // 使用 key 加载多个资源
        List<string> keys = new List<string>() { "characters", "animals" };
        // Operation handle used to load and release assets
        AsyncOperationHandle<IList<GameObject>>  loadHandle = Addressables.LoadAssetsAsync<GameObject>(
            keys, // Either a single key or a List of keys 
            null, 
            Addressables.MergeMode.Union, // How to combine multiple labels 
            false); // Whether to fail if any asset fails to load

        // Wait for the operation to finish in the background
        await loadHandle.Task;

        // Instantiate the results
        float x = 0, z = 0;
        foreach (var addressable in loadHandle.Result)
        {
            if (addressable != null)
            {
                Instantiate<GameObject>(addressable,
                    new Vector3(x++ * 2.0f, 0, z * 2.0f),
                    Quaternion.identity,
                    transform); // make child of this object

                if (x > 9)
                {
                    x = 0;
                    z++;
                }
            }
        }
    }
    public IEnumerator LoadWithLabel5()
    {
        // 使用 key 加载多个资源
        List<string> keys = new List<string>() { "characters", "animals" };
        AsyncOperationHandle<IList<GameObject>> loadHandle = Addressables.LoadAssetsAsync<GameObject>(
            keys, // Either a single key or a List of keys 
            addressable =>
            {
                // Called for every loaded asset
                Debug.Log(addressable.name);
            }, Addressables.MergeMode.Union, // How to combine multiple labels 
            false); // Whether to fail if any asset fails to load

        // Wait for the operation to finish in the background

        if (!loadHandle.IsDone)
        {
            // 注意：当使用 LoadAssetsAsync 加载一个资源时，上面的数据并不一定正确
            yield return loadHandle;
        }

        // Instantiate the results
        float x = 0, z = 0;
        foreach (var addressable in loadHandle.Result)
        {
            if (addressable != null)
            {
                Instantiate<GameObject>(addressable,
                    new Vector3(x++ * 2.0f, 0, z * 2.0f),
                    Quaternion.identity,
                    transform); // make child of this object

                if (x > 9)
                {
                    x = 0;
                    z++;
                }
            }
        }
    }

    /*
    {
        // Load the Prefabs
        var prefabOpHandle = Addressables.LoadAssetsAsync<GameObject>( keys, null, Addressables.MergeMode.Union, false);

        // Load a Scene additively
        var sceneOpHandle = Addressables.LoadSceneAsync(nextScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);

        await System.Threading.Tasks.Task.WhenAll(prefabOpHandle.Task, sceneOpHandle.Task);
    }
    */

    // Release asset when parent object is destroyed
    private void OnDestroy()
    {
        //Addressables.Release(handle);
        //Addressables.Release(loadHandle);

        // 切场景时 Unity 自动调用下面方法
        //Resources.UnloadUnusedAssets();
        //Resources.UnloadAsset();
    }


}
