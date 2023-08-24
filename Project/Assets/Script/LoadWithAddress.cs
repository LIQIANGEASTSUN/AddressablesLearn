using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadWithAddress : MonoBehaviour
{

    private string address = "Assets/Res/Sphere.prefab";

    private AsyncOperationHandle<GameObject> handle;

    // Start is called before the first frame update
    void Start()
    {
        handle = Addressables.LoadAssetAsync<GameObject>(address);
        handle.Completed += LoadComplete;

        GameObject panel = GameObject.Find("Panel");
        if (panel)
        {
            panel.SetActive(false);
        }
    }

    private void LoadComplete(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject go = GameObject.Instantiate(handle.Result);
        }
        else
        {
            Debug.LogError("Load Error");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        Addressables.Release(handle);
    }

}
