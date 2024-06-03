using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadContentCatalog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 加载 Catlog hash
    public IEnumerator StartLoad()
    {
        //Load a catalog and automatically release the operation handle.
        AsyncOperationHandle<IResourceLocator> handle = Addressables.LoadContentCatalogAsync("path_to_secondary_catalog", true);
        yield return handle;

        //...
    }

    // 如果调用了 Addressables.LoadContentCatalogAsync 其返回的 handle 在调用下面方法之前要释放掉，也可以传入 autoReleaseHandle = true
    // If you call UpdateCatalog without providing a list of catalogs, Addressables checks all the loaded catalogs for updates.
    IEnumerator UpdateCatalogs()
    {
        AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs();

        yield return updateHandle;
        Addressables.Release(updateHandle);
    }

    // You can also call Addressables.CheckForCatalogUpdates directly to get the list of catalogs that have updates and then perform the update:
    IEnumerator CheckCatalogs()
    {
        List<string> catalogsToUpdate = new List<string>();
        AsyncOperationHandle<List<string>> checkForUpdateHandle = Addressables.CheckForCatalogUpdates();
        checkForUpdateHandle.Completed += op => { catalogsToUpdate.AddRange(op.Result); };

        yield return checkForUpdateHandle;

        if (catalogsToUpdate.Count > 0)
        {
            AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate);
            yield return updateHandle;
            Addressables.Release(updateHandle);
        }

        Addressables.Release(checkForUpdateHandle);
    }
    // IMPORTANT
    // If you update a catalog when you have already loaded content from the related AssetBundles, there might be conflicts between the loaded AssetBundles and the updated versions.Enable the Unique Bundle Ids option in Addressable settings to stop the possibility of bundle ID collisions at runtime. Enabling this option also means that more AssetBundles must typically be rebuilt when you perform a content update. Refer to Content update builds for more information. You can also unload any content and AssetBundles that must be updated, which can be a slow operation.



}
