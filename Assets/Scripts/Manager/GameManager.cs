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

        public GameStateController gameController;

        public GameObject kinectControllerGO;
        public GameObject kinectCanvasPrefab;
        public Transform kinectCanvasTF;

        public bool isStart;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            if (gameController == null)
            {
                gameController = GameStateController.Instance;
            }
        }

        void Start()
        {
            gameController.ChangeState<GameGuid>(GameState.GameGuid);
        }

        void Update()
        {
            gameController.OnUpdate();
        }

        //初始化Kinect配置
        public void InitKinect()
        {
            kinectControllerGO=GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/GameObject/KinectController"));
            kinectCanvasTF = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/KinectCanvas")).transform;
            InteractionManager.Instance.guiHandCursor = kinectCanvasTF.Find("GUICursor").GetComponent<Image>();
        }
    }
}

