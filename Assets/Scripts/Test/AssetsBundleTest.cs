using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AssetsBundleTest : MonoBehaviour
{
    IEnumerator Start()
    {
        using (UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle("http://120.25.202.75/mnt/qr_img/cube.assetbundle"))
        {
            yield return req.SendWebRequest();
            if (req.error == null)
            {
                Debug.Log((req.downloadHandler as DownloadHandlerAssetBundle).assetBundle.name);
                Debug.Log(DownloadHandlerAssetBundle.GetContent(req).name);
            }
            else
            {
                Debug.Log("下载出错" + req.responseCode + "," + req.error);
            }
        }
    }
}