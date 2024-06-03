using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class LoadWithAddress : MonoBehaviour
{
    // Assign in Editor or in code
    public string address;

    // Retain handle to release asset and operation
    private AsyncOperationHandle<GameObject> handle;

    // Start the load operation on start
    void StartTTT()
    {
        handle = Addressables.LoadAssetAsync<GameObject>(address);
        handle.Completed += Handle_Completed;
    }

    async void StartTTT2()
    {
        handle = Addressables.LoadAssetAsync<GameObject>(address);
        // 使用 await 在同一帧等待
        await handle.Task;
        Instantiate(handle.Result, transform);
    }

    // Instantiate the loaded prefab on complete
    private void Handle_Completed(AsyncOperationHandle<GameObject> operation)
    {
        if (operation.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(operation.Result, transform);
        }
        else
        {
            Debug.LogError($"Asset for {address} failed to load.");
        }
    }

    public List<string> keys = new List<string>() { "characters", "animals" };
    private void LoadWithLabel()
    {
        AsyncOperationHandle<IList<GameObject>> loadHandle;
        float x = 0, z = 0;
        loadHandle = Addressables.LoadAssetsAsync<GameObject>(
            keys, // Either a single key or a List of keys 
            addressable =>
            {
                    //Gets called for every loaded asset
                    if (addressable != null)
                {
                    Instantiate<GameObject>(addressable,
                        new Vector3(x++ * 2.0f, 0, z * 2.0f),
                        Quaternion.identity,
                        transform);
                    if (x > 9)
                    {
                        x = 0;
                        z++;
                    }
                }
            }, Addressables.MergeMode.Union, // How to combine multiple labels 
            false); // Whether to fail if any asset fails to lo
    }

    // 使用 Label 加载每个资源的 IResourceLocation，然后通过 IResourceLocation 加载每一个资源
    Dictionary<string, GameObject> _preloadedObjects = new Dictionary<string, GameObject>();
    private IEnumerator PreloadHazardsWithLabel()
    {
        //find all the locations with label "SpaceHazards"
        var loadResourceLocationsHandle
            = Addressables.LoadResourceLocationsAsync("SpaceHazards", typeof(GameObject));

        if (!loadResourceLocationsHandle.IsDone)
            yield return loadResourceLocationsHandle;

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

    AsyncOperationHandle<GameObject> opHandle;

    public IEnumerator StartLoad()
    {
        opHandle = Addressables.LoadAssetAsync<GameObject>(address);

        // yielding when already done still waits until the next frame
        // so don't yield if done.
        if (!opHandle.IsDone)
        {
            yield return opHandle;
        }

        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(opHandle.Result, transform);
        }
        else
        {
            Addressables.Release(opHandle);
        }
    }

    // Operation handle used to load and release assets
    AsyncOperationHandle<IList<GameObject>> loadHandle;

    public async void StartSSS()
    {
        loadHandle = Addressables.LoadAssetsAsync<GameObject>(
            keys, // Either a single key or a List of keys 
            addressable =>
            {
                // Called for every loaded asset
                Debug.Log(addressable.name);
            }, Addressables.MergeMode.Union, // How to combine multiple labels 
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
    public IEnumerator Startttttt()
    {
        loadHandle = Addressables.LoadAssetsAsync<GameObject>(
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
            DownloadStatus downloadStatus = opHandle.GetDownloadStatus();
            long loadBytes = downloadStatus.DownloadedBytes;
            long total = downloadStatus.TotalBytes;
            // 下载总字节的百分比
            float percent = downloadStatus.Percent;
            // 比如总共下载 5 个资源，不管每个资源的字节多少，只按照 比例每一个资源占 20%
            // 比如 下载完成了 3 个 则返回 2 * 20% = 60%
            float percentComplete = opHandle.PercentComplete;
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
        Addressables.Release(handle);
        Addressables.Release(loadHandle);
    }
}
