using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

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

        private Dictionary<string, AssetBundle> loadedAssetBundles =
            new Dictionary<string, AssetBundle>();
        public Dictionary<string, AssetBundle> LoadedAssetBundles
        {
            get { return loadedAssetBundles; }
        }
        private Dictionary<string, int> referenceCounts = new Dictionary<string, int>();

        private AssetBundleManager() { }

        public AssetBundle LoadAssetBundle(string bundlePath)
        {
            if (!loadedAssetBundles.ContainsKey(bundlePath))
            {
                AssetBundle bundle = AssetBundle.LoadFromFile(
                    Path.Combine(Application.streamingAssetsPath, bundlePath)
                );
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

        public IEnumerator LoadAssetBundleFromServer(string bundleURL, string bundleName)
        {
            if (!loadedAssetBundles.ContainsKey(bundleName))
            {
                using (
                    UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(@bundleURL)
                )
                {
                    yield return request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
                        if (bundle != null)
                        {
                            loadedAssetBundles.Add(bundleName, bundle);
                            referenceCounts.Add(bundleName, 1);
                        }
                        else
                        {
                            Debug.Log("bundle为空");
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to load asset bundle from server: " + request.error);
                    }
                }
            }
            else
            {
                referenceCounts[bundleName]++;
                Debug.Log(referenceCounts[bundleName]);
            }
        }

        public T LoadAsset<T>(string bundlePath, string assetName)
            where T : Object
        {
            if (loadedAssetBundles.ContainsKey(bundlePath))
            {
                AssetBundle bundle = loadedAssetBundles[bundlePath];
                T asset = bundle.LoadAsset<T>(assetName);
                return asset;
            }
            Debug.LogWarning(assetName + "的资源不存在请检查资源名！");
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
