using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class DownloadDependenciesAsync : MonoBehaviour
{

    private IEnumerator DownloadDependencies()
    {
        string key = "assetKey";
        // Check the download size
        // 获取要下载的一个或者多个资源的总大小
        AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(key);
        yield return getDownloadSize;
        //If the download size is greater than 0, download all the dependencies.
        //If Addressables has already cached all the required AssetBundles, then Result is zero.
        if (getDownloadSize.Result > 0)
        {
            AsyncOperationHandle downloadDependencies = Addressables.DownloadDependenciesAsync(key);
            yield return downloadDependencies;
            Addressables.Release(downloadDependencies);
        }
    }

    private IEnumerator DownloadDependencies2()
    {
        string key = "assetKey";
        // Check the download size
        // 获取要下载的一个或者多个资源的总大小
        AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(key);
        yield return getDownloadSize;
        //If the download size is greater than 0, download all the dependencies.
        //If Addressables has already cached all the required AssetBundles, then Result is zero.
        if (getDownloadSize.Result > 0)
        {
            // 下载完成后自动释放句柄
            bool autoReleaseHandle = true;
            AsyncOperationHandle downloadDependencies = Addressables.DownloadDependenciesAsync(key, autoReleaseHandle);
            yield return downloadDependencies;
        }
    }

    private IEnumerator DownloadDependencies3()
    {
        string key = "assetKey";
        AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync(key);
        while (downloadHandle.Status == AsyncOperationStatus.None)
        {
            DownloadStatus downloadStatus = downloadHandle.GetDownloadStatus();
            long loadBytes = downloadStatus.DownloadedBytes;
            long total = downloadStatus.TotalBytes;
            // 下载总字节的百分比，比如总共下载 6 个资源，第一个资源的字节总数占比为 50%
            // 则下载完成第一个资源的时候，该操作已完成了 50%
            float percent = downloadStatus.Percent;
            // 比如总共下载 5 个资源，不管每个资源的字节多少，只按照 比例每一个资源占 20%
            // 比如 下载完成了 3 个 则返回 2 * 20% = 60%
            float percentComplete = downloadHandle.PercentComplete;

            yield return null;
        }

        Addressables.Release(downloadHandle);
    }

    // 清除缓存的 AssetBundle，清除后的资源需要重新下载
    // Addressables.ClearDependencyCacheAsync

    // 结合 LoadContentCatalog.cs 下载更新资源

}
