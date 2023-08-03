using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadWithLabels : MonoBehaviour
{

    public List<string> keys = new List<string>() { "HAHA", "hehe" };

    private AsyncOperationHandle<IList<GameObject>> handle;

    // Start is called before the first frame update
    void Start()
    {
        Addressables.LoadAssetsAsync<GameObject>(
            keys, // Either a single key or a List of keys 
            addressable => {
                //Gets called for every loaded asset
                if (addressable != null)
                {
                    Instantiate<GameObject>(addressable);
                }
            }, Addressables.MergeMode.Union, // How to combine multiple labels 
            false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
