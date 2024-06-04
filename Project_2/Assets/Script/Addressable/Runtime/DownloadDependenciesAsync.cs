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
        // ��ȡҪ���ص�һ�����߶����Դ���ܴ�С
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
        // ��ȡҪ���ص�һ�����߶����Դ���ܴ�С
        AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(key);
        yield return getDownloadSize;
        //If the download size is greater than 0, download all the dependencies.
        //If Addressables has already cached all the required AssetBundles, then Result is zero.
        if (getDownloadSize.Result > 0)
        {
            // ������ɺ��Զ��ͷž��
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
            // �������ֽڵİٷֱȣ������ܹ����� 6 ����Դ����һ����Դ���ֽ�����ռ��Ϊ 50%
            // ��������ɵ�һ����Դ��ʱ�򣬸ò���������� 50%
            float percent = downloadStatus.Percent;
            // �����ܹ����� 5 ����Դ������ÿ����Դ���ֽڶ��٣�ֻ���� ����ÿһ����Դռ 20%
            // ���� ��������� 3 �� �򷵻� 2 * 20% = 60%
            float percentComplete = downloadHandle.PercentComplete;

            yield return null;
        }

        Addressables.Release(downloadHandle);
    }

    // �������� AssetBundle����������Դ��Ҫ��������
    // Addressables.ClearDependencyCacheAsync

    // ��� LoadContentCatalog.cs ���ظ�����Դ

}
