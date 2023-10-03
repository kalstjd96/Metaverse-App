
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
            APIManager.Instance.CallRESTAPI(caseUrl, RESTAPI_TYPE.GET, (result, ackData) =>
            {
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
                    if (ackData.GetField("mobumYn").str != null)
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
                            });
                        }
                    }
                    else
                    {
                        UIPopup.Instance.OpenNoticePopup(new PopupInfo()
                        {
                            title = "알림",
                            content = "회원정보를 확인할 수 없습니다.",
                            okTitle = "확인",
                            onOkPressed = () => UIPopup.Instance.ClosePopup()
                        });
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
