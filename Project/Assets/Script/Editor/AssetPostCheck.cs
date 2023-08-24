using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetPostCheck : AssetPostprocessor
{

    // 纹理导入之前调用
    public void OnPreprocessTexture()
    {
        // 可以修改一些属性或者使用代码验证纹理的命名是否合乎规范
        TextureImporter textureImporter = assetImporter as TextureImporter;
        Debug.LogError("Pre Texture:" + textureImporter.assetPath);
    }

    // 纹理导入之后调用
    public void OnPostprocessTexture(Texture2D texture)
    {
        TextureImporter textureImporter = assetImporter as TextureImporter;
        Debug.LogError("Post Texutre:" + assetPath.ToLower());
        Debug.LogError("Post Texture:" + texture.width + "    " + texture.height);
    }

    // 所有资源导入都会调用这里
    public void OnPreprocessAsset()
    {
        //目录信息也会被获取到
        //Debug.LogError("Asset:" + assetImporter.assetPath);
    }

    /// <summary>
    /// 在任意数量的资源导入完成后调用
    /// 此函数必须声明为 static
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


