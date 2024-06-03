using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;

public class CleanBundleCache : Editor
{

    [MenuItem("Window/Asset Management/Addressables/Custom/CleanBundleCache")]
    public static void Clean()
    {
		HashSet<string> catalogsIds = new HashSet<string>();
		foreach (var locator in Addressables.ResourceLocators)
		{
			if (locator.LocatorId == "AddressablesMainContentCatalog")
			{
				catalogsIds.Add(locator.LocatorId);
				Debug.LogError("LocaterId:" + locator.LocatorId);
				break;
			}
		}

		if (catalogsIds.Count == 0)
			return;

		var cleanBundleCacheHandle = Addressables.CleanBundleCache(catalogsIds);
		cleanBundleCacheHandle.Completed += op =>
		{
			Debug.LogError("Clean Success");
			// during caching a reference is added to the catalogs.
			// release is needed to reduce the reference and allow catalog to be uncached for updating
			Addressables.Release(op);
		};
	}

}
