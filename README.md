# Metaverse-TaxSquare
<img src="https://capsule-render.vercel.app/api?type=waving&color=auto&height=200&section=header&text=Metaverse-TaxSquare&fontSize=80" /> 

# 메타버스 서울 내 택스스퀘어 

## Features (담당 기능)

-   [QR을 통한 딥링크 기능 구현](#qr-deeklink)
-   [월드맵 기능 구현](#world-map)
    
## QR DeepLink

>사용된 스크립트<br/>
> TaxDeepLink.cs

AppFlyer를 통한 딥링크 기능 구현을 담당하였습니다. (ThirdParty)

```c#

namespace SCM.Service.TaxSquare.ModelTaxPlayer
{
    public class TaxDeepLinkManager : MonoBehaviour
    {
        #region Non_public
        private AppsFlyerObjectScript appsFlyerObject;
        AvatarPartCategory[] avatarPart_10Parts;
        private const string caseUrl = "tax/mobum";
        private const string DeepLinkError = "Deep link Error";
        private const string DeepLinkNotFound = "Deep link Not Found.";
        #endregion

        private void Awake()
        {
            appsFlyerObject = GetComponent<AppsFlyerObjectScript>();
            avatarPart_10Parts = (AvatarPartCategory[])Enum.GetValues(typeof(AvatarPartCategory));
        }

        private void Start()
        {
            StartCoroutine(CheckForDeepLinks());
        }

        /// <summary>
        /// QR 접근 여부 확인
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckForDeepLinks()
        {
            float timeoutDuration = 5f;
            float elapsedTime = 0f;

            while (true)
            {
                if (elapsedTime >= timeoutDuration)
                    break;

                if (appsFlyerObject.DidReceivedDeepLink)
                {
                    appsFlyerObject.DidReceivedDeepLink = false;
                    CheckForModelTaxPlayer();
                    break;
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// QR을 통해 앱 접근 시 진행
        /// </summary>
        private void CheckForModelTaxPlayer()
        {
            UIPopup.Instance.ShowLoading();
            APIManager.Instance.CallRESTAPIUsedToken(caseUrl, RESTAPI_TYPE.GET, (result, ackData) =>
            {
                UIPopup.Instance.HideLoading();
                if (result == APIResult.ERROR)
                {
                    UIPopup.Instance.OpenNoticePopup(new PopupInfo()
                    {
                        title = "알림",
                        content = "에러 발생 \n 다시 시도해주세요. \n",
                        okTitle = "확인",
                        onOkPressed = () => UIPopup.Instance.ClosePopup()
                    });
                }
                else
                {
                    string resultMessage = ackData.GetField("mobumYn").str;
                    if (resultMessage.Equals("Y"))
                        StartCoroutine(UnlockModelTaxPlayerItemPartsCoroutine());
                    else if (resultMessage.Equals("N"))
                    {
                        UIPopup.Instance.OpenNoticePopup(new PopupInfo()
                        {
                            title = "알림",
                            content = "모범납세자가 아닙니다.",
                            okTitle = "확인",
                            onOkPressed = () => UIPopup.Instance.ClosePopup()
                        }); ;
                    }
                }
            });
        }

        /// <summary>
        /// 아바타 파츠 아이템 락 해제
        /// </summary>
        private IEnumerator UnlockModelTaxPlayerItemPartsCoroutine()
        {
            string deepLinkInformation = ShowDeepLinkParams();
            int itemIndex = int.Parse(deepLinkInformation.Split(':')[0]);
            AvatarPartCategory[] avatarPartCategoryIndex = new AvatarPartCategory[deepLinkInformation.Split(':').Length - 1];

            for (int i = 0; i < avatarPartCategoryIndex.Length; i++)
            {
                avatarPartCategoryIndex[i] = avatarPart_10Parts[int.Parse(deepLinkInformation.Split(':')[i + 1].Trim())];
            }

            int routineCounter = 0;
            bool hasError = false;

            foreach (var category in avatarPartCategoryIndex)
            {
                routineCounter += 1;
                AvatarManager.Instance.AcquirePart(itemIndex, category, (result, errorMsg) =>
                {
                    routineCounter -= 1;
                    if (result != APIResult.SUCCESS)
                        hasError = true;
                });
            }

            yield return null;
            yield return new WaitUntil(() => routineCounter <= 0);

            if (hasError)
            {
                UIPopup.Instance.OpenNoticePopup(new PopupInfo()
                {
                    title = "알림",
                    content = "획득 실패 다시 시도해주세요",
                    okTitle = "확인",
                    onOkPressed = () => UIPopup.Instance.ClosePopup()
                });
            }
            else
            {
                UIPopup.Instance.OpenNoticePopup(new PopupInfo()
                {
                    title = "알림",
                    content = "아이템을 획득했습니다. 아이템 창을 확인하세요.",
                    okTitle = "확인",
                    onOkPressed = () => UIPopup.Instance.ClosePopup()
                });
            }
        }

        /// <summary>
        /// AppsFlyer 대시보드 입력된 아바타 파츠 데이터 값 가져오기
        /// </summary>
        /// <returns>partIndex:AvatarPartCategory Number_01:AvatarPartCategory Number_02:...</returns>
        private string ShowDeepLinkParams()
        {
            Dictionary<string, object> deepLinkParams = appsFlyerObject.DeepLinkParams;
            string text = null;

            if (deepLinkParams != null)
            {
                if (deepLinkParams.ContainsKey("deep_link_error"))
                    text = DeepLinkError;
                else if (deepLinkParams.ContainsKey("deep_link_not_found"))
                    text = DeepLinkNotFound;
                else
                {
                    foreach (KeyValuePair<string, object> entry in deepLinkParams)
                    {
                        if (entry.Value != null)
                            text += entry.Value.ToString() + ':';
                    }
                }
            }

            if (text != null && text.EndsWith(":"))
                text = text.Substring(0, text.Length - 1);

            return text;
        }
    }
}
```

## World Map

>사용된 스크립트<br/>
> GraphicsLineRenderer.cs 

데이터를 기반으로 Line Graph Viewer 기능을 구현하였습니다.
```c#

namespace SCM.Service.TaxSquare.Common.TaxWorldMap
{
    public class WorldMapController : MonoBehaviour
    {
        private IWorldMapState worldMapState;
        public TaxWorldMapInfo taxWorldMapInfo;

        public event Action OnWorldMapCanvasOpened;
        ...

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            ResetEvent();
            tZoneCloseButton.onClick.AddListener(() => OnClicktZoneCloseButton?.Invoke());
            aZoneCloseButton.onClick.AddListener(() => OnClickaZoneCloseButton?.Invoke());
            xZoneCloseButton.onClick.AddListener(() => OnClickxZoneCloseButton?.Invoke());
            ...
            BindEvent();
        }

        public void ResetEvent()
        {
            tZoneCloseButton.onClick.RemoveAllListeners();
            aZoneCloseButton.onClick.RemoveAllListeners();
            xZoneCloseButton.onClick.RemoveAllListeners();
            ...
        }

        private void BindEvent()
        {
            OnClicktZoneCloseButton += OnClickCloseButton;
            OnClickaZoneCloseButton += OnClickCloseButton;
            OnClickxZoneCloseButton += OnClickCloseButton;
            ...
        }

        public void SetState(SCENE state, bool isOpen)
        {
            if (!isOpen)
            {
                CloseTaxWorldMapCanvas();
                return;
            }
            else
            {
                int currentScene = (int)state;

                if (currentScene >= 60001 && currentScene <= 60030)
                {
                    // SCENE.TaxCity_Tzone
                    currentCanvas = tZoneWorldMapPanel;
                    worldMapState = new TZoneWorldMapState(this, taxWorldMapInfo, tZoneWorldMapPanel, onClickScene);
                }
                ...

                ShowWorldMap();
            }

        }

        public void OnClickCloseButton()
        {
            CommonUIPopup.Instance.OpenTotalWorldMapCanvas(false);
            currentCanvas.enabled = false;
        }

        public void OnClickSpaceViewButton()
        {
            CommonUIPopup.Instance.OpenWorldMapCanvas(false);
            CommonUIPopup.Instance.OpenSpaceSelectCanvas(true);
            currentCanvas.enabled = false;
        }

        public void SceneMoveEvent()
        {
            currentCanvas.enabled = false;
            SceneLoadManager.Instance.LoadScene(worldMapState.onClickScene);
            CommonUIPopup.Instance.OpenTotalWorldMapCanvas(false);
        }

        public void ShowWorldMap()
        {
            worldMapState.ShowWorldMap();
        }

        public void CloseTaxWorldMapCanvas()
        {
            if (currentCanvas != null)
                currentCanvas.enabled = false;
        }
    }
}

namespace SCM.Service.TaxSquare.Common.TaxWorldMap
{
    public class AZoneWorldMapState : IWorldMapState
    {
        private WorldMapController controller;
        private TaxWorldMapInfo taxWorldMapInfo;
        private Canvas canvas;

        public SCENE onClickScene { get; private set; }

        SCENE IWorldMapState.onClickScene
        {
            get { return onClickScene; }
            set { onClickScene = value; }
        }

        public AZoneWorldMapState(WorldMapController controller, TaxWorldMapInfo taxWorldMapInfo, Canvas targetCanvas, SCENE onClickScene)
        {
            this.controller = controller;
            this.taxWorldMapInfo = taxWorldMapInfo;
            this.canvas = targetCanvas;
            this.onClickScene = onClickScene;
        }

        public void ShowWorldMap()
        {
            canvas.enabled = true;
            CommonUIPopup.Instance.OpenSpaceSelectCanvas(false);
            HandlePointClick(canvas);
        }

        public void HandlePointClick(Canvas targetPanel)
        {
            WorldMapPointUIItem[] mapPointItems = targetPanel.GetComponentsInChildren<WorldMapPointUIItem>();

            for (int i = 0; i < mapPointItems.Length; i++)
            {
                mapPointItems[i].GetComponent<Button>().onClick.RemoveAllListeners();
                mapPointItems[i].SetTaxItem(taxWorldMapInfo.azoneMapPoints[i], OnPointClick);
            }
        }
        public void OnPointClick(TaxMapPoint arg0)
        {
            controller.aZoneThumbnailImage.sprite = arg0.thumbnail;
            controller.aZonePointTitleText.text = arg0.pointName;
            controller.aZonePointInfoText.text = arg0.description;
            onClickScene = arg0.targetScene;
        }
    }
}


```
![GraphViewer](https://github.com/kalstjd96/Unity_CyberPlantAR/assets/47016363/4a15b007-6a0b-413e-bb4d-35d637f9f657)

