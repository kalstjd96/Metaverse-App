# Metaverse-TaxSquare
<img src="https://capsule-render.vercel.app/api?type=waving&color=auto&height=200&section=header&text=Metaverse-TaxSquare&fontSize=80" /> 

# 메타버스 서울 내 택스스퀘어 

## Features (담당 기능)

-   [QR을 통한 딥링크 기능 구현](#qr-deeklink)
-   [Graph Viewer 구현](#graphicsLine-setting)
    
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

## GraphicsLine Setting

>사용된 스크립트<br/>
> GraphicsLineRenderer.cs 

데이터를 기반으로 Line Graph Viewer 기능을 구현하였습니다.
```c#

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class GraphicsLineRenderer : MonoBehaviour
{
    public Material lmat;
    private Mesh ml;
    private Vector3 s;

    private float lineSize = 0.1f;
    private bool firstQuad = true;

    public void SetMesh(Mesh ml)
    {
        GetComponent<MeshFilter>().mesh = ml;
    }

    public void SetMaterial(Material lmat)
    {
        GetComponent<MeshRenderer>().material = lmat;
    }

    public void setWidth(float width)
    {
        lineSize = width;
    }

    public void AddPoint(Vector3 point)
    {
        if (s != Vector3.zero)
        {
            AddLine(GetComponent<MeshFilter>().mesh, MakeQuad(s, point, lineSize, firstQuad));
            firstQuad = false;
        }

        s = point;
    }

    Vector3[] MakeQuad(Vector3 s, Vector3 e, float w, bool all)
    {
        w = w / 2;

        Vector3[] q;
        if (all)
        {
            q = new Vector3[4];
        }
        else
        {
            q = new Vector3[2];
        }

        Vector3 n = Vector3.Cross(s, e);
        Vector3 l = Vector3.Cross(n, e - s);
        l.Normalize();

        if (all)
        {
            q[0] = transform.InverseTransformPoint(s + l * w);
            q[1] = transform.InverseTransformPoint(s + l * -w);
            q[2] = transform.InverseTransformPoint(e + l * w);
            q[3] = transform.InverseTransformPoint(e + l * -w);
        }
        else
        {
            q[0] = transform.InverseTransformPoint(s + l * w);
            q[1] = transform.InverseTransformPoint(s + l * -w);
        }
        return q;
    }

    void AddLine(Mesh m, Vector3[] quad)
    {
        int vl = m.vertices.Length;

        Vector3[] vs = m.vertices;
        vs = resizeVertices(vs, 2 * quad.Length);

        for (int i = 0; i < 2 * quad.Length; i += 2)
        {
            vs[vl + i] = quad[i / 2];
            vs[vl + i + 1] = quad[i / 2];
        }

        Vector2[] uvs = m.uv;
        uvs = resizeUVs(uvs, 2 * quad.Length);

        if (quad.Length == 4)
        {
            uvs[vl] = Vector2.zero;
            uvs[vl + 1] = Vector2.zero;
            uvs[vl + 2] = Vector2.right;
            uvs[vl + 3] = Vector2.right;
            uvs[vl + 4] = Vector2.up;
            uvs[vl + 5] = Vector2.up;
            uvs[vl + 6] = Vector2.one;
            uvs[vl + 7] = Vector2.one;
        }
        else
        {
            if (vl % 8 == 0)
            {
                uvs[vl] = Vector2.zero;
                uvs[vl + 1] = Vector2.zero;
                uvs[vl + 2] = Vector2.right;
                uvs[vl + 3] = Vector2.right;

            }
            else
            {
                uvs[vl] = Vector2.up;
                uvs[vl + 1] = Vector2.up;
                uvs[vl + 2] = Vector2.one;
                uvs[vl + 3] = Vector2.one;
            }
        }

        int tl = m.triangles.Length;

        int[] ts = m.triangles;
        ts = resizeTriangles(ts, 12);

        if (quad.Length == 2)
        {
            vl -= 4;
        }

        // front-facing quad
        ts[tl] = vl;
        ts[tl + 1] = vl + 2;
        ts[tl + 2] = vl + 4;

        ts[tl + 3] = vl + 2;
        ts[tl + 4] = vl + 6;
        ts[tl + 5] = vl + 4;

        // back-facing quad
        ts[tl + 6] = vl + 5;
        ts[tl + 7] = vl + 3;
        ts[tl + 8] = vl + 1;

        ts[tl + 9] = vl + 5;
        ts[tl + 10] = vl + 7;
        ts[tl + 11] = vl + 3;

        m.vertices = vs;
        m.uv = uvs;
        m.triangles = ts;
        m.RecalculateBounds();
        m.RecalculateNormals();
    }

    Vector3[] resizeVertices(Vector3[] ovs, int ns)
    {
        Vector3[] nvs = new Vector3[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++)
        {
            nvs[i] = ovs[i];
        }

        return nvs;
    }

    Vector2[] resizeUVs(Vector2[] uvs, int ns)
    {
        Vector2[] nvs = new Vector2[uvs.Length + ns];
        for (int i = 0; i < uvs.Length; i++)
        {
            nvs[i] = uvs[i];
        }

        return nvs;
    }

    int[] resizeTriangles(int[] ovs, int ns)
    {
        int[] nvs = new int[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++)
        {
            nvs[i] = ovs[i];
        }

        return nvs;
    }
}

```
![GraphViewer](https://github.com/kalstjd96/Unity_CyberPlantAR/assets/47016363/4a15b007-6a0b-413e-bb4d-35d637f9f657)

