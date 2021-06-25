using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace CustomScripts.Util
{
    public class BundleLoader : MonoBehaviour
    {
        public static List<T> LoadAssetBundle<T>(string bundleName) where T : Object
        {
            var assets = new List<T>();
            // var CurrentPlatform = EditorUserBuildSettings.activeBuildTarget.ToString();
            var currentPlatform = "Android";

            AssetBundle packAssetBundleList = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, currentPlatform, bundleName));
            string[] assetNames = packAssetBundleList.GetAllAssetNames();

            foreach (var assetName in assetNames)
            {
                T asset = packAssetBundleList.LoadAsset(assetName) as T;
                assets.Add(asset);
            }

            packAssetBundleList.Unload(false); 

            return assets;
        }
    }
}
