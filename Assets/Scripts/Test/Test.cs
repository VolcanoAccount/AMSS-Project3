using UnityEngine;
using System.Collections.Generic;
using AMSS;
using System;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class Test : MonoBehaviour
{
    public Transform[] countdown;
    public RawImage imageDisplay; // 用于显示图片的RawImage组件

    void Start()
    {
        // ClothesPosInfoJson wrapper =JsonParser<ClothesPosInfoJson>.Parse("JsonInfo/ClothesPositionJson");
        // Debug.Log(wrapper.ClothesPosinfoList);
        // 遍历数组并输出信息
        PohtoShooter.CountdownAndMakePhoto(countdown, this, Camera.main, OnPhotoTaken);
        // LoadAndDisplayImage("screenshot.png"); // 替换为您要加载的图片名称
    }

    // 从Application.persistentDataPath加载图片并显示在RawImage上
    private void LoadAndDisplayImage(string imageName)
    {
        // 获取图片的完整路径
        string imagePath = Path.Combine(Application.persistentDataPath, "Screenshots", imageName);
        Debug.Log("图片路径: " + imagePath);

        // 检查图片是否存在
        if (File.Exists(imagePath))
        {
            // 读取图片的二进制数据
            byte[] imageData = File.ReadAllBytes(imagePath);

            // 创建Texture2D，并从二进制数据中加载图片
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(imageData);

            // 将Texture2D显示在RawImage组件上
            imageDisplay.texture = texture;
        }
        else
        {
            Debug.LogWarning("图片不存在: " + imagePath);
        }
    }

    private void OnPhotoTaken(byte[] photoByte)
    {
        // Texture2D texture = new Texture2D(1, 1);
        // texture.LoadImage(photoByte);
        // imageDisplay.texture=texture;
    }
}
