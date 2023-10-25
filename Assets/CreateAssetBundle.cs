#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;

public class CreateAssetBundle : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create Asset Bundles")]
    private static void BuildAllAssetBundles()
    {
        string assetBundleDirectoryPath = Application.dataPath + "/AssetBundles";
        try
        {
            BuildPipeline.BuildAssetBundles(assetBundleDirectoryPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
            Debug.Log("Asset Bundles created successfully.");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Error creating Asset Bundles: {e}");
        }
    }
#endif
}
