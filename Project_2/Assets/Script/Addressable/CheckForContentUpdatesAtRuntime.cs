using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForContentUpdatesAtRuntime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Addressables 1.21.21 -> Manual -> BuildContent -> Content update builds -> Check for content updates at runtime
    // You can add a custom script to periodically check whether there are new Addressables content updates. Use the following function call to start the update:
    // List\<string\> contains the list of modified locator IDs. You can filter this list to only update specific IDs, or pass it entirely into the UpdateCatalogs API.
    // 检查需要更新的资源 id
    //[public static AsyncOperationHandle<List<string>> CheckForCatalogUpdates(bool autoReleaseHandle = true)]

    // If there is new content, you can either present the user with a button to perform the update, or do it automatically. It's up to you to make sure that stale Assets are released.
    // The list of catalogs can be null and if so, the following script updates all catalogs that need an update:
    // The return value is the list of updated locators.
    //[public static AsyncOperationHandle<List<IResourceLocator>> UpdateCatalogs(IEnumerable<string> catalogs = null, bool autoReleaseHandle = true)]

    // You might also want to remove any bundle cache entries that are no longer referenced as a result of updating the catalogs. If so, use this version of the UpdateCatalogs API instead where you can enable the additional parameter autoCleanBundleCache to remove any unneeded cache data:
    //[public static AsyncOperationHandle<List<IResourceLocator>> UpdateCatalogs(bool autoCleanBundleCache, IEnumerable<string> catalogs = null, bool autoReleaseHandle = true)]


}
