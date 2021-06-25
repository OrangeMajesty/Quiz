using System.IO;
using UnityEngine;
using UnityEditor;

public class BundleAssetsCreate : MonoBehaviour
{
    private static string _path = "Assets/StreamingAssets/";

    [MenuItem("Bundle/Build Android")]
    static void BuildBundleAndroid()
    {
        // Build Target Android
        Build(BuildTarget.Android);
    }

    [MenuItem("Bundle/Build All")]
    static void BuildBundleAll()
    {
        // Build Target Android
        Build(BuildTarget.Android);

        // Build Target Windows
        Build(BuildTarget.StandaloneWindows);
    }

    static void Build(BuildTarget target)
    {
        MakeDir(_path + target);
        BuildPipeline.BuildAssetBundles(_path + target, BuildAssetBundleOptions.None, target);
    }

    static void MakeDir(string path)
    {
        string fullPath = path;
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }
    }
}
