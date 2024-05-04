using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System.IO;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

public class AddressableGroupEditor
{
    public static void AutoSetGroup(string groupName, string lableName, string assetPath, bool isSimplied = false)
    {
        var set = AddressableAssetSettingsDefaultObject.Settings; 
        AddressableAssetGroup group = set.FindGroup(groupName); 
        if (group == null) 
        {
            bool setAsDefaultGroup = false;
            bool readOnly = false;
            bool postEvent = true;
            group = set.CreateGroup(groupName, setAsDefaultGroup, readOnly, postEvent, null, typeof(BundledAssetGroupSchema));
        }
        string guid = AssetDatabase.AssetPathToGUID(assetPath);  //获取指定路径下资源的 GUID（全局唯一标识符）
        AddressableAssetEntry asset = set.CreateOrMoveEntry(guid, group); 
        if (isSimplied)
        {
            asset.address = Path.GetFileNameWithoutExtension(assetPath);
        }
        else 
        { 
            asset.address = assetPath; 
        }

        if (!string.IsNullOrEmpty(lableName))
        {
            asset.SetLabel(lableName, true, true);
        }
    }

}
