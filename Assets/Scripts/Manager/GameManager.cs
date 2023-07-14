using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AMSS
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance { get; private set; }

        public UIManager uIManager { get; private set; }

        public KinectManager kinectManager { get; private set; }

        public GameStateController gameController;

        public GameObject kinectControllerPrefab;
        public GameObject kinectCanvasPrefab;
        Transform kinectCanvasTF;
        public GameObject GuidVideoPrefab;
        GameObject GuidVideoGO;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            if (uIManager == null)
            {
                uIManager = UIManager.Instance;
            }

            if (kinectManager == null)
            {
                kinectManager = KinectManager.Instance;
            }
            if (gameController == null)
            {
                gameController=GameStateController.Instance;
            }
        }

        void Start()
        {
            // if (kinectManager && KinectManager.IsKinectInitialized())
            // {

            // }
            GameObject.Instantiate<GameObject>(kinectControllerPrefab);
            kinectCanvasTF = GameObject.Instantiate<GameObject>(kinectCanvasPrefab).transform;
            GuidVideoGO = GameObject.Instantiate<GameObject>(GuidVideoPrefab);
            GuidVideoGO.SetActive(false);
            InteractionManager.Instance.guiHandCursor = kinectCanvasTF.Find("GUICursor").GetComponent<Image>();

            uIManager.PushPanel(UIPanelType.GameStart);

        }

        void Update()
        {
            gameController.OnUpdate();
        }

        public void PlayGuidVideo()
        {
            GuidVideoGO.SetActive(true);
        }
    }
}

