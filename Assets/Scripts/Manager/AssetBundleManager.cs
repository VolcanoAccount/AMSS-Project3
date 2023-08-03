using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace AMSS
{   
    /// <summary>
    /// AB包资源管理类
    /// </summary>
    public class AssetBundleManager
    {
        private static AssetBundleManager instance;
        public static AssetBundleManager Instance
        {
            get
            {
                instance ??= new AssetBundleManager();
                return instance;
            }
        }

        //存储已下载的ab包资源
        private Dictionary<string, AssetBundle> loadedAssetBundles =
            new Dictionary<string, AssetBundle>();
        public Dictionary<string, AssetBundle> LoadedAssetBundles
        {
            get { return loadedAssetBundles; }
        }

        //记录每个资源包被引用的次数
        private Dictionary<string, int> referenceCounts = new Dictionary<string, int>();

        private AssetBundleManager() { }

        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <param name="bundlePath">资源包名字</param>
        /// <returns>返回下载的ab包</returns>
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

        /// <summary>
        /// 从服务器上拉取AB包
        /// </summary>
        /// <param name="bundleURL"></param>
        /// <param name="bundleName">资源包名字</param>
        /// <returns></returns>
        public IEnumerator LoadAssetBundleFromServer(string bundleURL, string bundleName)
        {
            if (!loadedAssetBundles.ContainsKey(bundleName))
            {
                using UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(@bundleURL);
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
