# Metaverse-TaxSquare
<img src="https://capsule-render.vercel.app/api?type=waving&color=auto&height=200&section=header&text=Metaverse-TaxSquare&fontSize=80" /> 

# 메타버스 서울 내 택스스퀘어 

## Features (담당 기능)

-   [QR을 통한 딥링크 기능 구현](#qr-deeklink)
-   [월드맵 기능 구현](#world-map)
-   [TaxGPT 기능 구현](#taxgpt-chatting)
-   [세금 조회 및 납부](#etax-inquiry)
    
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
> WorldMapController.cs, AZoneWorldMapState.cs

택스스퀘어 내 월드맵 Viewer, 원하는 장소를 클릭하여 해당 장소로 이동하는 기능을 구현하였습니다.

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

 [TaxGPT 기능 구현](#taxgpt-chatting)
    
## TaxGPT Chatting

>사용된 스크립트<br/>
> TaxGptUI.cs

챗 GPT와 같은 기능을 하는 현대 사회에 맞는 최신의 세금 정보를 알려주는 기능입니다.

```c#

...
private IEnumerator GetRequest(TaxGPTAnswerItem answerItem, string question)
{
    yield return null;
    string caseUrl = $"tax/chats/ask?query={UnityWebRequest.EscapeURL(question)}&streaming=true&chat_id=ecac18eb-0cbd-4963-9afb-39c510944c7e";
    string url = $"{APIServerSettings.Instance.RootURL}{caseUrl}";
    
    using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
    {
        webRequest.timeout = 60;

        string token = TokenManager.Instance.AccessToken;
        if (token is not null)
            webRequest.SetRequestHeader("Authorization", token);

        yield return webRequest.SendWebRequest();

        StopCoroutine(cursorCoroutine);

        if (webRequest.result != UnityWebRequest.Result.Success)
            answerItem.SetItem("TaxGPT 연결이 끊겼습니다.<br>다시 시도하세요");
        else
        {
            string responseJson = webRequest.downloadHandler.text;
            string[] responseDataArray = responseJson.Split("data:");

            foreach (string responseData in responseDataArray)
            {
                JSONObject jsonObject = new JSONObject(responseData);
                if (jsonObject.HasField("result") && jsonObject.GetField("result").type == JSONObject.Type.STRING)
                {
                    string resultMsg = jsonObject.GetField("result").str;
                    if (resultMsg != null)
                    {
                        string decodedString = Regex.Unescape(resultMsg);
                        if (!responseData.Equals(responseDataArray[responseDataArray.Length - 1]))
                            decodedString = AddRandomCharacter(decodedString);
                        PrintAnswer(answerItem, decodedString);

                        yield return new WaitForSeconds(0.1f);
                    }
                }
            }
        }

        chatInput.interactable = true;
        sendButton.interactable = true;
    }
}

IEnumerator Cursor(TaxGPTAnswerItem chatbotAnswerItem)
{
    while (true)
    {
        chatbotAnswerItem.SetItem("|");
        yield return new WaitForSeconds(0.5f);
        chatbotAnswerItem.SetItem(" ");
        yield return new WaitForSeconds(0.5f);
    }
}

private string AddRandomCharacter(string isCursor)
{
    char randomChar = (char)Random.Range(0, 1);

    if (randomChar != 0)
        isCursor += "|";
    else isCursor += "";

    return isCursor;
}
...

```

## ETax Inquiry

>사용된 스크립트<br/>
> ETaxInquiry.cs

ETAX와 연계하여 나의 세금 정보를 조회하고 납부하는 기능을 구현하였습니다.

```c#
namespace SCM.Service.TaxSquare.ETax
{
    public class ETaxInquiry : MonoBehaviour
    {
        [SerializeField] private TMP_Text taxCount;
        [SerializeField] private RectTransform listOpenImage;
        [SerializeField] private RectTransform paymentListPanel;
        ...

        private bool isListOpen;
        private string ETAXUrl = ;
        float paymentListPanelOriginalPosition;

        private void Awake() => Initialize();

        private void Initialize()
        {
            isListOpen = false;
            SetButtonOnClickListeners();
            SetPaymentListPanelSize();
        }

        private void SetButtonOnClickListeners()
        {
            taxInquiryListViewButton.onClick.AddListener(InquiryListOpenButtonClick);
            taxPaymentWebViewButton.onClick.AddListener(TaxPaymentWebViewOpen);
        }

        private void SetPaymentListPanelSize()
        {
            paymentListPanelOriginalPosition = paymentListPanel.rect.height;
            paymentListPanel.sizeDelta = new Vector2(paymentListPanel.rect.width, 0);
        }

        private void OnDisable() => ResetListAndPanel();

        private void ResetListAndPanel()
        {
            isListOpen = false;
            paymentListPanel.gameObject.SetActive(false);
            ClearTaxScrollViewContent();
            ResetListOpenImageRotation();
        }

        private void ResetListOpenImageRotation()
        {
            if (listOpenImage.localEulerAngles.z != -180)
            {
                Vector3 rotation = listOpenImage.localEulerAngles;
                rotation.z = -180;
                listOpenImage.localEulerAngles = rotation;
            }
        }

        public void GetTaxInquiryInformation(string jsonResponse) => LoadTaxInquiryInformation(jsonResponse);

        private void LoadTaxInquiryInformation(string jsonResponse)
        {
            ClearTaxScrollViewContent();

            TaxInfoList taxInfoList = JsonUtility.FromJson<TaxInfoList>(jsonResponse);
            taxCount.text = $"{taxInfoList.allCount}건";

            LoadData(taxInfoList);
        }

        private void ClearTaxScrollViewContent()
        {
            foreach (Transform child in taxScrollViewContent)
            {
                Destroy(child.gameObject);
            }
        }

        private void LoadData(TaxInfoList taxInfoList)
        {
            string etaxUrlAdd = $"pay_link/SM/q_title//checkable/Y/tax_info/{taxInfoList.allCount}";

            foreach (TaxInfo taxInfo in taxInfoList.taxInf)
            {
                GameObject taxInformationItem = Instantiate(taxInquiryitem, taxScrollViewContent);
                GameObject taxInformationText = taxInformationItem.transform.GetChild(0).gameObject;
                SetTaxInformationTextValues(taxInformationText, taxInfo);
                etaxUrlAdd += $"@@{taxInfo.tpayNo}@@{taxInfo.validAmt}";
            }

            ETAXUrl += etaxUrlAdd;
        }

        private void SetTaxInformationTextValues(GameObject taxInformationItem, TaxInfo taxInfo)
        {
            TMP_Text[] values = taxInformationItem.GetComponentsInChildren<TMP_Text>();
            values[0].text = taxInfo.tpayNo;
            values[1].text = taxInfo.epayNo;
            values[2].text = taxInfo.validAmt;
        }

        private void InquiryListOpenButtonClick() => TogglePanel();

        private void TogglePanel()
        {
            if (!isListOpen)
            {
                paymentListPanel.gameObject.SetActive(true);
            }

            AdjustPanelSizeAndRotation();
        }

        private void AdjustPanelSizeAndRotation()
        {
            float targetHeight = isListOpen ? 0 : paymentListPanelOriginalPosition;
            Vector2 newSize = new Vector2(paymentListPanel.sizeDelta.x, targetHeight);

            float rotationAngle = !isListOpen ? -90 : -180;
            Vector3 rotation = listOpenImage.localEulerAngles;
            rotation.z = rotationAngle;

            AnimatePanelAdjustments(newSize, rotation);
        }

        private void AnimatePanelAdjustments(Vector2 newSize, Vector3 rotation)
        {
            listOpenImage.DORotate(rotation, 0.5f);
            paymentListPanel.DOSizeDelta(newSize, 0.5f).SetEase(Ease.OutQuad).OnComplete(PanelOnOff);
        }

        private void PanelOnOff()
        {
            isListOpen = !isListOpen;
            paymentListPanel.gameObject.SetActive(isListOpen);
        }

        private void TaxPaymentWebViewOpen() => OpenTaxPaymentWebView();

        private void OpenTaxPaymentWebView()
        {
            webViewObject.gameObject.SetActive(true);
            StartCoroutine(webViewObject.ETAXPaymentWebView(ETAXUrl));
            gameObject.SetActive(false);
            //eTaxServicePanel.SetActive(false);
        }

        [System.Serializable]
        private class TaxInfo
        {
            public string validAmt;
            public string epayNo;
            public string tpayNo;
            public string gubun;
        }

        [System.Serializable]
        private class TaxInfoList
        {
            public int allCount;
            public TaxInfo[] taxInf;
        }
    }
}
```
