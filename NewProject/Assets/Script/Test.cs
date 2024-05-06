using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string path = UnityEngine.AddressableAssets.Addressables.BuildPath;
        Debug.LogError("Path:" + path);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
