using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;

public class AssetBundleUploader : EditorWindow
{
    private string serverUrl = "http://120.25.202.75/mnt/qr_img"; // URL
    private string assetBundleFolderPath = "Assets/AssetBundles"; // AssetBundle 文件夹路径

    [MenuItem("Tools/Upload AssetBundles")]
    public static void ShowWindow()
    {
        AssetBundleUploader window = EditorWindow.GetWindow<AssetBundleUploader>();
        window.titleContent = new GUIContent("Upload AssetBundles");
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Server URL:");
        serverUrl = EditorGUILayout.TextField(serverUrl);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("AssetBundle Folder Path:");
        assetBundleFolderPath = EditorGUILayout.TextField(assetBundleFolderPath);

        EditorGUILayout.Space();

        if (GUILayout.Button("Upload AssetBundles"))
        {
            UploadAssetBundles();
        }
    }

    private void UploadAssetBundles()
    {
        string[] assetBundleFiles = Directory.GetFiles(assetBundleFolderPath, "*.unity3d");

        foreach (string filePath in assetBundleFiles)
        {
            string fileName = Path.GetFileName(filePath);
            string url = serverUrl + "?filename=" + Uri.EscapeUriString(fileName);
            byte[] fileData = File.ReadAllBytes(filePath);

            UnityWebRequest request = UnityWebRequest.Put(url, fileData);
            request.method = UnityWebRequest.kHttpVerbPOST;

            // 使用异步方式发送请求
            AsyncOperation asyncOperation = request.SendWebRequest();

            // 等待请求完成
            while (!asyncOperation.isDone)
            {
                // 可以在这里更新进度条或显示上传进度
                float progress = asyncOperation.progress;
                Debug.Log("Uploading progress: " + progress);
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Uploaded AssetBundle: " + fileName);
            }
            else
            {
                Debug.LogError("Failed to upload AssetBundle: " + fileName);
                Debug.LogError(request.error);
            }

            request.Dispose();
        }

        Debug.Log("AssetBundle upload completed!");
    }
}
