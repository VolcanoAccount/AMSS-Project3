using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AMSS
{
    public class AssetBundleManager
    {
        private static AssetBundleManager instance;
        public static AssetBundleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AssetBundleManager();
                }
                return instance;
            }
        }

        private Dictionary<string, AssetBundle> loadedAssetBundles = new Dictionary<string, AssetBundle>();
        private Dictionary<string, int> referenceCounts = new Dictionary<string, int>();

        private AssetBundleManager() { }

        public AssetBundle LoadAssetBundle(string bundlePath)
        {
            if (!loadedAssetBundles.ContainsKey(bundlePath))
            {
                AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundlePath));
                if (bundle != null)
                {
                    loadedAssetBundles.Add(bundlePath, bundle);
                    referenceCounts.Add(bundlePath, 1);
                    return bundle;
                }
            }
            else
            {
                referenceCounts[bundlePath]++;
                Debug.Log(referenceCounts[bundlePath]);
                return loadedAssetBundles[bundlePath];
            }
            return null;
        }

        public T LoadAsset<T>(string bundlePath, string assetName) where T : Object
        {
            if (loadedAssetBundles.ContainsKey(bundlePath))
            {
                AssetBundle bundle = loadedAssetBundles[bundlePath];
                T asset = bundle.LoadAsset<T>(assetName);
                return asset;
            }
            return null;
        }

        public void UnloadAssetBundle(string bundlePath)
        {
            if (loadedAssetBundles.ContainsKey(bundlePath))
            {
                referenceCounts[bundlePath]--;
                if (referenceCounts[bundlePath] <= 0)
                {
                    loadedAssetBundles[bundlePath].Unload(true);
                    loadedAssetBundles.Remove(bundlePath);
                    referenceCounts.Remove(bundlePath);
                }
            }
        }
    }
}

