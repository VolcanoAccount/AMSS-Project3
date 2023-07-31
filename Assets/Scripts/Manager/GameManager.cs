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

        public bool isInitKinect { get; private set; }

        float totalTime = 60f;

        [SerializeField, Header("无玩家操作时长")]
        private float currentTime = 0;
        private bool isTimerRunning = false;

        public bool isGaming = false;

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
            gameController.ChangeState<GamePrepare>(GameState.GamePrepare);
        }

        void Update()
        {
            gameController.OnUpdate();
            if (isTimerRunning)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= totalTime)
                {
                    gameController.ChangeState<GameOver>(GameState.GameOver);
                    StopTimer();
                    isGaming = false;
                }
            }
        }

        //初始化Kinect配置
        public void InitKinect()
        {
            kinectControllerGO = GameObject.Instantiate(
                Resources.Load<GameObject>("Prefabs/GameObject/KinectController")
            );
            kinectCanvasTF = GameObject
                .Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/KinectCanvas"))
                .transform;
            InteractionManager.Instance.guiHandCursor = kinectCanvasTF
                .Find("GUICursor")
                .GetComponent<Image>();
            PlayerGestureListener.Instance.gestureInfo = kinectCanvasTF
                .Find("GestureInfo")
                .GetComponent<Text>();
            isInitKinect = true;
        }

        public void StartTimer()
        {
            isTimerRunning = true;
        }

        public void StopTimer()
        {
            isTimerRunning = false;
            currentTime = 0f;
        }

        public bool GameOver()
        {
            return UIManager.Instance.ClearAllPanel();
        }
    }
}
