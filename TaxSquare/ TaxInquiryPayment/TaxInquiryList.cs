
public class TaxInquiryList : MonoBehaviour
{
    #region Public Fields
    const string caseUrl = "tax/taxCI";
    string ETAXUrl = "...";
    WebView webViewObject;

    [System.Serializable]
    public class TaxInfo
    {
        public string validAmt;
        public string epayNo;
        public string tpayNo;
        public string gubun;
    }
    [System.Serializable]
    public class TaxInfoList
    {
        public int allCount;
        public TaxInfo[] taxInf;
    }
    #endregion

    #region Unity Methods

    private void Start()
    {
        CheckForModelTaxPlayer();
    }
    #endregion

    public void TaxInquiry(string jsonResponse)
    {
        TaxInfoList taxInfoList = JsonUtility.FromJson<TaxInfoList>(jsonResponse);

        foreach (TaxInfo taxInfo in taxInfoList.taxInf)
        {
            string etaxurlAdd = "...";
            int eTaxCount = taxInfoList.allCount;
            for (int i = 0; i < eTaxCount; i++)
            {
                etaxurlAdd += "@@" + taxInfo.tpayNo + "@@" + taxInfo.validAmt;
            }

            ETAXUrl += etaxurlAdd;

            Debug.Log("ETAXUrl : " + ETAXUrl);
            StartCoroutine(webViewObject.ETAXPaymentWebView(ETAXUrl));
        }
    }
    private void CheckForModelTaxPlayer()
    {
        APIManager.Instance.CallRESTAPI(caseUrl, RESTAPI_TYPE.GET,(res, body)=>
        {
            if (res == APIResult.ERROR)
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
                TaxInquiry(body.ToString());
        });
    }
}
