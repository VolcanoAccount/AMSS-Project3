using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class ImageUploader : MonoBehaviour
{
    // 服务器上传图片的URL
    private string serverURL = "https://your-server-url/upload_image.php"; // 替换为实际的服务器端接口URL

    // 在游戏中调用此方法来上传图片
    public void UploadImageToServer(string imageName)
    {
        // 获取图片的完整路径
        string imagePath = Path.Combine(Application.persistentDataPath, imageName);

        // 检查图片是否存在
        if (File.Exists(imagePath))
        {
            // 读取图片的二进制数据
            byte[] imageData = File.ReadAllBytes(imagePath);

            // 开始上传图片
            StartCoroutine(UploadImage(imageData));
        }
        else
        {
            Debug.LogWarning("图片不存在: " + imagePath);
        }
    }

    // 使用UnityWebRequest上传图片数据到服务器
    private System.Collections.IEnumerator UploadImage(byte[] imageData)
    {
        // 创建UnityWebRequest对象，并设置上传数据
        using (UnityWebRequest www = UnityWebRequest.Post(serverURL, new WWWForm()))
        {
            www.SetRequestHeader("Content-Type", "image/png"); // 设置图片类型

            // 将图片数据添加到UnityWebRequest
            www.uploadHandler = new UploadHandlerRaw(imageData);
            www.uploadHandler.contentType = "image/png";

            // 发送请求并等待服务器响应
            yield return www.SendWebRequest();

            // 检查是否出现错误
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("图片上传错误: " + www.error);
            }
            else
            {
                Debug.Log("图片上传成功!");
                // 在这里可以根据服务器返回的响应进行适当的处理
            }
        }
    }
}
