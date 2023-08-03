using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadTest : MonoBehaviour
{
    public AssetReference assetReference;

    // Start is called before the first frame update
    void Start()
    {
        AsyncOperationHandle handle = assetReference.LoadAssetAsync<GameObject>();
        handle.Completed += LoadComplete;
    }

    private GameObject go;
    private void LoadComplete(AsyncOperationHandle handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            go = GameObject.Instantiate(assetReference.Asset) as GameObject;
        }
        else
        {
            Debug.LogError("LoadError");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

        }
    }

    private void OnDestroy()
    {
        assetReference.ReleaseAsset();
    }

}
