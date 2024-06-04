using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;

public class CheckUpdateAndDownload : MonoBehaviour
{
    // Addressables 1.21.21 -> Manual -> BuildContent -> Content update builds -> Check for content updates at runtime
    // By default, the Addressables system manages the catalog automatically at runtime.
    // If you built your application with a remote catalog,
    // the Addressables system automatically checks for a new catalog,
    // and downloads the new version and loads it into memory.
    // You can add a custom script to periodically check whether there are new Addressables content updates.
    // Use the following function call to start the update:
    // List<string> contains the list of modified locator IDs.
    // You can filter this list to only update specific IDs, or pass it entirely into the UpdateCatalogs API.
    // 检查需要更新的资源 id
    //[public static AsyncOperationHandle<List<string>> CheckForCatalogUpdates(bool autoReleaseHandle = true)]
    // You can also call Addressables.CheckForCatalogUpdates directly to get the list of catalogs that have updates and then perform the update:

    // Unique Bundle IDs setting
    // If you want to update content at runtime rather than at application startup, use the Unique Bundle IDs setting.Enabling this setting can make it easier to load updated AssetBundles in the middle of an application session, but can make builds slower and updates larger
    // Assets\AddressableAssetsData\AddressableAssetSettings.asset  -> Build -> Unique Bundle IDs

    // If you want to change the default catalog update behavior of the Addressables system,
    // you can disable the automatic check and check for updates manually.
    // Refer to Updating catalogs for more information.

    // IMPORTANT
    // If you update a catalog when you have already loaded content from the related AssetBundles,
    // there might be conflicts between the loaded AssetBundles and the updated versions.
    // Enable the Unique Bundle Ids option in Addressable settings to stop the possibility of bundle ID collisions at runtime.
    // Enabling this option also means that more AssetBundles must typically be rebuilt when you perform a content update.
    // Refer to Content update builds for more information.
    // You can also unload any content and AssetBundles that must be updated, which can be a slow operation.

    // 内容在 Addressable 1.21.21 -> Manual -> Use Addressables at runtime -> Manage catalogs at runtime

    private AsyncOperationStatus status = AsyncOperationStatus.None;
    private System.Exception operationException = null;

    // 需要更新的 Catlog 文件
    private List<string> updateCatlogList = new List<string>();
    private AsyncOperationHandle<List<IResourceLocator>> updateCatalogHandle;
    private IList<IResourceLocator> downloadList;
    private List<object> keyList = new List<object>();

    public IEnumerator StartUpdate()
    {
        yield return InitializeAsync();
        if (!CheckOperationStatus(status, "InitializeAsync", operationException)) {
            yield break;
        }

        yield return CheckForCatalogUpdates();
        if (!CheckOperationStatus(status, "CheckForCatalogUpdates", operationException))
        {
            yield break;
        }

        yield return UpdateCatalogs();
        if (!CheckOperationStatus(status, "UpdateCatalogs", operationException))
        {
            yield break;
        }

        yield return GetDownloadSizeAsync();
        if (!CheckOperationStatus(status, "GetDownloadSizeAsync", operationException))
        {
            yield break;
        }

        yield return DownloadDependenciesAsync();

        if (status == AsyncOperationStatus.Succeeded)
        {
            // 下载完毕，开始游戏逻辑
        }

    }

    // 初始化调用
    private IEnumerator InitializeAsync()
    {
        AsyncOperationHandle<IResourceLocator> initHandle = Addressables.InitializeAsync();
        yield return initHandle;
        status = initHandle.Status;
        operationException = initHandle.OperationException;
        Addressables.Release(initHandle);
    }

    // 检测 Catalog 文件，获取需要更新的 Catalog 列表
    private IEnumerator CheckForCatalogUpdates()
    {
        AsyncOperationHandle<List<string>> checkCatalogHandle = Addressables.CheckForCatalogUpdates();
        yield return checkCatalogHandle;
        updateCatlogList = new List<string>(checkCatalogHandle.Result);
        status = checkCatalogHandle.Status;
        operationException = checkCatalogHandle.OperationException;
        Addressables.Release(checkCatalogHandle);
    }

    private bool CheckOperationStatus(AsyncOperationStatus status, string source, System.Exception exception)
    {
        if (this.status != AsyncOperationStatus.Succeeded)
        {
            DebugError(source + "  Error:" + exception.ToString());
        }
        return status == AsyncOperationStatus.Succeeded;
    }

    public IEnumerator UpdateCatalogs()
    {
        // 没有需要更新的 Catalog
        if (updateCatlogList.Count <= 0)
        {
            yield break;
        }

        // 更新 Catalog 文件
        updateCatalogHandle = Addressables.UpdateCatalogs(updateCatlogList);
        yield return updateCatalogHandle;
        downloadList = new List<IResourceLocator>(updateCatalogHandle.Result);
        status = updateCatalogHandle.Status;
        operationException = updateCatalogHandle.OperationException;
        Addressables.Release(updateCatalogHandle);
    }

    private IEnumerator GetDownloadSizeAsync()
    {
        // 获取下载资源总大小
        long totalDownloadSize = 0;
        foreach (var locator in downloadList)
        {
            // 获取下载的文件大小
            AsyncOperationHandle<long> sizeHandle = Addressables.GetDownloadSizeAsync(locator.Keys);
            yield return sizeHandle;
            if (sizeHandle.Status != AsyncOperationStatus.Succeeded)
            {
                status = sizeHandle.Status;
                operationException = sizeHandle.OperationException;
                Addressables.Release(sizeHandle);
                yield break;
            }

            if (sizeHandle.Result > 0)
            {
                totalDownloadSize += sizeHandle.Result;
                keyList.AddRange(locator.Keys);
            }
            Addressables.Release(sizeHandle);
        }
        yield return null;
    }

    private IEnumerator DownloadDependenciesAsync()
    {
        // 下载更新资源
        AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync(keyList);
        while (!downloadHandle.IsDone)
        {
            if (downloadHandle.Status == AsyncOperationStatus.Failed)
            {
                DebugError("DownloadDependenciesAsync Error:" + downloadHandle.OperationException.ToString());
                Addressables.Release(downloadHandle);
                yield break;
            }

            // 下载进度
            DownloadStatus downloadStatus = downloadHandle.GetDownloadStatus();
            // 获取下载资源的字节数
            long loadBytes = downloadStatus.DownloadedBytes;
            // 获取下载资源的总字节数
            long total = downloadStatus.TotalBytes;
            // 下载总字节的百分比，比如总共下载 6 个资源，第一个资源的字节总数占比为 50%
            // 则下载完成第一个资源的时候，该操作已完成了 50%
            float percent = downloadStatus.Percent;
            // 比如总共下载 5 个资源，不管每个资源的字节多少，只按照 比例每一个资源占 20%
            // 比如 下载完成了 3 个 则返回 2 * 20% = 60%
            float percentComplete = downloadHandle.PercentComplete;

            yield return null;
        }

        yield return downloadHandle;
        Addressables.Release(downloadHandle);
        status = downloadHandle.Status;


    }

    private void DebugError(string message)
    {
        Debug.LogError(message);
    }

    // 清除缓存的 AssetBundle，清除后的资源需要重新下载
    // Addressables.ClearDependencyCacheAsync

    // 加载 Catlog hash
    //public IEnumerator StartLoad()
    //{
    //    //Load a catalog and automatically release the operation handle.
    //    AsyncOperationHandle<IResourceLocator> handle = Addressables.LoadContentCatalogAsync("path_to_secondary_catalog", true);
    //    yield return handle;
    //    //...
    //    // 如果调用了 Addressables.LoadContentCatalogAsync 其返回的 handle 在调用 Addressables.UpdateCatalogs 之前要释放掉，
    //    // 也可以传入 在 LoadContentCatalogAsync 方法传入 autoReleaseHandle = true
    //}
}