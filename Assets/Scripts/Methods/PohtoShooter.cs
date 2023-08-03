using System;
using System.Collections;
using UnityEngine;

namespace AMSS
{
    public static class PohtoShooter
    {
        /// <summary>
        /// 开启协程倒计时并拍照
        /// </summary>
        public static void CountdownAndMakePhoto(
            Transform[] countdown,
            MonoBehaviour monoBehaviour,
            Camera backroundCamera,
            Action<byte[]> callback
        )
        {
            monoBehaviour.StartCoroutine(
                CoCountdownAndMakePhoto(monoBehaviour, countdown, backroundCamera, callback)
            );
        }

        //倒计时协程
        static IEnumerator CoCountdownAndMakePhoto(
            MonoBehaviour monoBehaviour,
            Transform[] countdown,
            Camera backroundCamera,
            Action<byte[]> callback
        )
        {
            if (countdown != null && countdown.Length > 0)
            {
                for (int i = 0; i < countdown.Length; i++)
                {
                    if (countdown[i])
                        countdown[i].gameObject.SetActive(true);

                    yield return new WaitForSeconds(1.0f);

                    if (countdown[i])
                        countdown[i].gameObject.SetActive(false);
                }
            }
            countdown[0].parent.gameObject.SetActive(false);
            InteractionManager.Instance.guiHandCursor.gameObject.SetActive(false);
            byte[] photoByte = MakePhoto(monoBehaviour, backroundCamera);
            callback?.Invoke(photoByte);
            yield return null;
        }

        /// <summary>
        /// 截取屏幕图像为Texture2D，并返回Texture2D的字节数组
        /// </summary>
        /// <param name="backroundCamera">渲染图像的相机</param>
        /// <returns>图像的二进制字节数组</returns>
        public static byte[] MakePhoto(MonoBehaviour monoBehaviour, Camera backroundCamera)
        {
            int resWidth = Screen.width;
            int resHeight = Screen.height;

            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false); //Create new texture
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);

            if (backroundCamera && backroundCamera.enabled)
            {
                backroundCamera.targetTexture = rt;
                backroundCamera.Render();
                backroundCamera.targetTexture = null;
            }

            // 获取截屏
            RenderTexture prevActiveTex = RenderTexture.active;
            RenderTexture.active = rt;

            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

            RenderTexture.active = prevActiveTex;

            byte[] btScreenShot = screenShot.EncodeToPNG();
            ImageUploader.UploadImageToServer(
                monoBehaviour,
                "http://120.25.202.75:8084/imgUpDown",
                btScreenShot
            );
            return btScreenShot;
        }
    }
}
