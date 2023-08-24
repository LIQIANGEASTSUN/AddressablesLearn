using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetPostCheck : AssetPostprocessor
{

    // ������֮ǰ����
    public void OnPreprocessTexture()
    {
        // �����޸�һЩ���Ի���ʹ�ô�����֤����������Ƿ�Ϻ��淶
        TextureImporter textureImporter = assetImporter as TextureImporter;
        Debug.LogError("Pre Texture:" + textureImporter.assetPath);
    }

    // ������֮�����
    public void OnPostprocessTexture(Texture2D texture)
    {
        TextureImporter textureImporter = assetImporter as TextureImporter;
        Debug.LogError("Post Texutre:" + assetPath.ToLower());
        Debug.LogError("Post Texture:" + texture.width + "    " + texture.height);
    }

    // ������Դ���붼���������
    public void OnPreprocessAsset()
    {
        //Ŀ¼��ϢҲ�ᱻ��ȡ��
        //Debug.LogError("Asset:" + assetImporter.assetPath);
    }

    /// <summary>
    /// ��������������Դ������ɺ����
    /// �˺�����������Ϊ static
    /// </summary>
    /// <param name="importedAssets"></param>
    /// <param name="deletedAssets"></param>
    /// <param name="movedAssets"></param>
    /// <param name="movedFromAssetPaths"></param>
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (var asset in importedAssets)
        {
            if (asset.EndsWith(".prefab"))
            {
                PostPrefab(asset);
            }
            //Debug.LogError("import:" + asset);
        }

        //foreach (var asset in deletedAssets)
        //{
        //    Debug.LogError("delete:" + asset);
        //}

        //foreach (var asset in movedAssets)
        //{
        //    Debug.LogError("move:" + asset);
        //}

        //foreach (var asset in movedFromAssetPaths)
        //{
        //    Debug.LogError("moveFromAsset:" + asset);
        //}
    }

    static void PostPrefab(string assetPath)
    {
        string groupName = "TestGroup2";
        string lableName = string.Empty;
        AddressableGroupEditor.AutoSetGroup( groupName, lableName, assetPath);
    }


}


