using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    Button returnBtn;
    Button downloadBtn;
    RawImage photo;
    RawImage qrCode;

    #region  面板周期函数
    public override void OnEnter()
    {
        base.OnEnter();
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;

        if (photo == null)
        {
            photo = transform.Find("PhotoFrame/Photo").GetComponent<RawImage>();
        }
        if (downloadBtn == null)
        {
            downloadBtn = transform.Find("DownloadBtn").GetComponent<Button>();
        }
        downloadBtn.gameObject.SetActive(true);
        photo.texture = PlayerManager.Instance.pohtoTex;
    }

    void Start()
    {
        qrCode = transform.Find("PhotoFrame/QrCode").GetComponent<RawImage>();
        qrCode.gameObject.SetActive(false);
        returnBtn = transform.Find("ReturnBtn").GetComponent<Button>();
        returnBtn.onClick.AddListener(OnClickReturnBtn);

        downloadBtn.onClick.AddListener(OnClickDownloadBtn);
    }

    void Update() { }

    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }

    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }

    public override void OnExit()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }
    #endregion

    void OnClickReturnBtn()
    {
        qrCode.gameObject.SetActive(false);
        photo.gameObject.SetActive(true);
        UIManager.Instance.PopPanel();
    }

    void OnClickDownloadBtn()
    {
        photo.gameObject.SetActive(false);
        qrCode.gameObject.SetActive(true);
        StartCoroutine(GenerateQRCode());
        downloadBtn.gameObject.SetActive(false);
    }

    IEnumerator GenerateQRCode()
    {
        using (
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(
                "http://120.25.202.75/mnt/qrcode/qr/qr.jpg"
            )
        )
        {
            yield return request.SendWebRequest();
            if (request.error == null)
            {
                qrCode.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            }
            else
            {
                Debug.Log("下载出错" + request.responseCode + "," + request.error);
            }
        }
    }
}
