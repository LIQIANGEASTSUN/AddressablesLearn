using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    public string key; // address string
    private AsyncOperationHandle<SceneInstance> loadHandle;

    void Start()
    {
        // ÇÐ»»³¡¾°
        loadHandle = Addressables.LoadSceneAsync(key, LoadSceneMode.Additive);
    }

    void OnDestroy()
    {
        // Ð¶ÔØ³¡¾° ¾ä±ú
        Addressables.UnloadSceneAsync(loadHandle);
    }

}
