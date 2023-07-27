using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AssetsBundleTest : MonoBehaviour
{
    public RawImage image;
    IEnumerator Start()
    {
        // using (UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle(@"http://120.25.202.75:8083/StandaloneWindows/cube.assetbundle"))
        // {
        //     yield return req.SendWebRequest();
        //     if (req.error == null)
        //     {
        //         Debug.Log((req.downloadHandler as DownloadHandlerAssetBundle).assetBundle.name);
        //         Debug.Log(DownloadHandlerAssetBundle.GetContent(req).name);
        //     }
        //     else
        //     {
        //         Debug.Log("下载出错" + req.responseCode + "," + req.error);
        //     }
        // }
        using (UnityWebRequest request=UnityWebRequestTexture.GetTexture("http://120.25.202.75/mnt/qrcode/qr/qr.jpg"))
        {
            yield return request.SendWebRequest();
            if (request.error == null)
            {
                // Debug.Log((request.downloadHandler as DownloadHandlerTexture).texture.name);
                // Debug.Log(DownloadHandlerTexture.GetContent(request).name);
                image.texture=((DownloadHandlerTexture)request.downloadHandler).texture;
            }
            else
            {
                Debug.Log("下载出错" + request.responseCode + "," + request.error);
            }
        }
    }
}