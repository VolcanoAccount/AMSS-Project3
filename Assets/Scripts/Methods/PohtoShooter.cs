using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AMSS
{
    public static class PohtoShooter
    {
        /// <summary>
        /// /// Counts down (from 3 for instance), then takes a picture and opens it
        /// </summary>
        public static void CountdownAndMakePhoto(Transform[] countdown, MonoBehaviour monoBehaviour, Camera backroundCamera)
        {
            monoBehaviour.StartCoroutine(CoCountdownAndMakePhoto(countdown, backroundCamera));
        }

        // counts down (from 3 for instance), then takes a picture and opens it
        private static IEnumerator CoCountdownAndMakePhoto(Transform[] countdown, Camera backroundCamera)
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

            MakePhoto(backroundCamera);
            yield return null;
        }

        /// <summary>
        /// Saves the screen image as png picture, and then opens the saved file.
        /// </summary>
        public static void MakePhoto(Camera backroundCamera)
        {
            MakePhoto(true, backroundCamera);
        }

        /// <summary>
        /// Saves the screen image as png picture, and optionally opens the saved file.
        /// </summary>
        /// <returns>The file name.</returns>
        /// <param name="openIt">If set to <c>true</c> opens the saved file.</param>
        public static string MakePhoto(bool openIt, Camera backroundCamera)
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

            // get the screenshot
            RenderTexture prevActiveTex = RenderTexture.active;
            RenderTexture.active = rt;

            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

            // clean-up
            RenderTexture.active = prevActiveTex;

            byte[] btScreenShot = screenShot.EncodeToJPG();

            // save the screenshot as jpeg file
            string sDirName = Application.persistentDataPath + "/Screenshots";
            if (!Directory.Exists(sDirName))
                Directory.CreateDirectory(sDirName);

            string sFileName = sDirName + "/" + "/screenshot.jpg";
            File.WriteAllBytes(sFileName, btScreenShot);

            Debug.Log("Photo saved to: " + sFileName);

            // open file
            if (openIt)
            {
                System.Diagnostics.Process.Start(sFileName);
            }

            return sFileName;
        }
    }
}

