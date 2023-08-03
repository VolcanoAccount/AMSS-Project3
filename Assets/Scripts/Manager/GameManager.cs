using UnityEngine;
using UnityEngine.UI;

namespace AMSS
{
    /// <summary>
    /// 游戏管理类
    /// 启动ui框架和游戏有限状态机框架
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameStateController GameController { get; private set; }

        public UIManager UIManager { get; private set; }

        public PlayerManager PlayerManager { get; private set; }

        public GameObject kinectControllerGO { get; private set; }
        Transform kinectCanvasTF;

        public bool isInitKinect { get; private set; }

        float totalTime = 60f;

        [SerializeField, Header("无玩家操作时长")]
        private float currentTime = 0;
        private bool isTimerRunning = false;

        public bool isGaming = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            if (GameController == null)
            {
                GameController = GameStateController.Instance;
            }

            if (UIManager == null)
            {
                UIManager = UIManager.Instance;
            }

            if (PlayerManager == null)
            {
                PlayerManager = PlayerManager.Instance;
            }
        }

        void Start()
        {
            GameController.ChangeState<GamePrepare>(GameState.GamePrepare);
        }

        void Update()
        {
            GameController.OnUpdate();
            if (isTimerRunning)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= totalTime)
                {
                    GameController.ChangeState<GameOver>(GameState.GameOver);
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
            kinectCanvasTF.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            kinectCanvasTF.GetComponent<Canvas>().worldCamera = Camera.main;
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
            return UIManager.ClearAllPanel();
        }

        //打开UIPanel
        public BasePanel PushPanel(UIPanelType type)
        {
            return UIManager.PushPanel(type);
        }

        //获取UI面板UIPanelType类型
        public UIPanelType GetPanelType(BasePanel panel)
        {
            return UIManager.GetPanelType(panel);
        }

        //关闭UI面板
        public void PopPanel()
        {
            UIManager.PopPanel();
        }
    }
}
