using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

public static class ImageUploader
{
    // 在游戏中调用此方法来上传图片
    public static void UploadImageToServer(
        MonoBehaviour monoBehaviour,
        string serverURL,
        byte[] imageData
    )
    {
        monoBehaviour.StartCoroutine(UploadImageToOSS(serverURL, imageData));
    }

    /// <summary>
    /// 图片上传到OSS
    /// </summary>
    /// <param name="serverURL"></param>
    /// <param name="imageData"></param>
    /// <returns></returns>
    public static IEnumerator UploadImageToOSS(string serverURL, byte[] imageData)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", imageData);
        UnityWebRequest webRequest = UnityWebRequest.Post(serverURL, form);

        yield return webRequest.SendWebRequest();
        if (
            webRequest.result == UnityWebRequest.Result.ProtocolError
            || webRequest.result == UnityWebRequest.Result.ConnectionError
        ) //如果其 请求失败，或是 网络错误
        {
            Debug.Log("图片上传错误: " + webRequest.error);
        }
        else
        {
            Debug.Log("图片上传成功!");
        }
        webRequest.Dispose();
    }
}
