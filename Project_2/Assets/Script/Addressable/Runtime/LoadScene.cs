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
        // �л�����
        loadHandle = Addressables.LoadSceneAsync(key, LoadSceneMode.Additive);
    }

    void OnDestroy()
    {
        // ж�س��� ���
        Addressables.UnloadSceneAsync(loadHandle);
    }

}
