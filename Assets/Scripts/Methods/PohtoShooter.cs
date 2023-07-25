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
        public static void CountdownAndMakePhoto(Transform[] countdown, MonoBehaviour monoBehaviour,Camera backroundCamera)
        {
            monoBehaviour.StartCoroutine(CoCountdownAndMakePhoto(countdown,backroundCamera));
        }

        // counts down (from 3 for instance), then takes a picture and opens it
        private static IEnumerator CoCountdownAndMakePhoto(Transform[] countdown,Camera backroundCamera)
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
            MakePhoto(true,backroundCamera);
        }

        /// <summary>
        /// Saves the screen image as png picture, and optionally opens the saved file.
        /// </summary>
        /// <returns>The file name.</returns>
        /// <param name="openIt">If set to <c>true</c> opens the saved file.</param>
        public static string MakePhoto(bool openIt,Camera backroundCamera)
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

#if !UNITY_WSA
            // save the screenshot as jpeg file
            string sDirName = Application.persistentDataPath + "/Screenshots";
            if (!Directory.Exists(sDirName))
                Directory.CreateDirectory(sDirName);

            string sFileName = sDirName + "/" + string.Format("{0:F0}", Time.realtimeSinceStartup * 10f) + ".jpg";
            File.WriteAllBytes(sFileName, btScreenShot);

            Debug.Log("Photo saved to: " + sFileName);

            // open file
            if (openIt)
            {
                System.Diagnostics.Process.Start(sFileName);
            }

            return sFileName;
#elif NETFX_CORE
        System.Threading.Tasks.Task<string> task = null;

        string sFileName = string.Format("{0:F0}", Time.realtimeSinceStartup * 10f) + ".jpg";
        string sFileUrl = string.Empty; // "ms-appdata:///local/" + sFileName;

		UnityEngine.WSA.Application.InvokeOnUIThread(() =>
		{
        	task = SaveImageFileAsync(sFileName, btScreenShot, openIt);
		}, true);

        while (task != null && !task.IsCompleted && !task.IsFaulted)
        {
            task.Wait(100);
        }

        if (task != null)
        {
            if (task == null)
                throw new Exception("Could not create task for SaveImageFileAsync()");
            else if (task.IsFaulted)
                throw task.Exception;

            sFileUrl = task.Result;
            Debug.Log(sFileUrl);
        }

        return sFileUrl;
#else
		return string.Empty;
#endif
        }

#if NETFX_CORE
    private async System.Threading.Tasks.Task<string> SaveImageFileAsync(string imageFileName, byte[] btImageContent, bool openIt)
    {
        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        Windows.Storage.StorageFile imageFile = await storageFolder.CreateFileAsync(imageFileName,
            Windows.Storage.CreationCollisionOption.ReplaceExisting);

        await Windows.Storage.FileIO.WriteBytesAsync(imageFile, btImageContent);

        if(openIt)
        {
            await Windows.System.Launcher.LaunchFileAsync(imageFile);
        }

        return imageFile.Path;
    }
#endif
    }
}

