using UnityEngine;

public class KeyboardHeightManager : MonoBehaviour
{
    [SerializeField] private RectTransform canvasScalerHeight;//CanvasScaler가 설정되어 있는 대상 - 설정에 따라 유동적으로 높이를 계산하기 위함
    [SerializeField] private RectTransform inputFieldRectTransform;
    [SerializeField] private RectTransform scrollViewViewport;
    private Vector2 originalInputFieldAnchoredPosition;
    private Vector2 originalScrollViewOffsetMin;

    private bool isKeyboardActive = false;

    private void Awake()
    {
        originalInputFieldAnchoredPosition = inputFieldRectTransform.anchoredPosition;
        originalScrollViewOffsetMin = scrollViewViewport.offsetMin;
    }

    private void Update()
    {
        if (IsKeyboardVisible())
        {
            isKeyboardActive = true;
            AdjustPositionForKeyboard();
        }
        else
        {
            if (!isKeyboardActive)
                return;

            isKeyboardActive = false;
            RestoreOriginalPosition();
        }
    }

    private bool IsKeyboardVisible()
    {
        return TouchScreenKeyboard.visible;
    }

    /// <summary>
    /// 키보드에 따라 UI 요소의 위치를 조정하는 메서드
    /// </summary>
    public void AdjustPositionForKeyboard()
    {
        int keyboardHeight = GetRelativeKeyboardHeight(canvasScalerHeight, false);

        // Adjust InputField's anchored position
        Vector2 newInputFieldPosition = originalInputFieldAnchoredPosition;
        newInputFieldPosition.y = keyboardHeight + 3f;
        inputFieldRectTransform.anchoredPosition = newInputFieldPosition;

        // Adjust ScrollView's viewport offsetMin
        Vector2 newOffsetMin = originalScrollViewOffsetMin;
        newOffsetMin.y = keyboardHeight;
        scrollViewViewport.offsetMin = newOffsetMin;

    }

    private void RestoreOriginalPosition()
    {
        inputFieldRectTransform.anchoredPosition = originalInputFieldAnchoredPosition;
        scrollViewViewport.offsetMin = originalScrollViewOffsetMin;
    }

    public static int GetRelativeKeyboardHeight(RectTransform rectTransform, bool includeInput)
    {
        int keyboardHeight = GetKeyboardHeight(includeInput);
        float screenToRectRatio = Screen.height / rectTransform.rect.height;
        float keyboardHeightRelativeToRect = keyboardHeight / screenToRectRatio;

        return (int)keyboardHeightRelativeToRect;
    }

    /// <summary>
    /// 모바일 키보드 높이 계산
    /// </summary>
    /// <param name="includeInput">Mobile input 높이 포함 여부</param>
    /// <returns></returns>
    public static int GetKeyboardHeight(bool includeInput)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
		using ( var unityClass = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" ) )
		{
			var currentActivity = unityClass.GetStatic<AndroidJavaObject>( "currentActivity" );
			var unityPlayer = currentActivity.Get<AndroidJavaObject>( "mUnityPlayer" );
			var view = unityPlayer.Call<AndroidJavaObject>( "getView" );

			if ( view == null ) return 0;

			int result;

			using ( var rect = new AndroidJavaObject( "android.graphics.Rect" ) )
			{
				view.Call( "getWindowVisibleDisplayFrame", rect );
				result = Screen.height - rect.Call<int>( "height" );
			}

			if ( !includeInput ) return result;

			var softInputDialog = unityPlayer.Get<AndroidJavaObject>( "mSoftInputDialog" );
			var window = softInputDialog?.Call<AndroidJavaObject>( "getWindow" );
			var decorView = window?.Call<AndroidJavaObject>( "getDecorView" );

			if ( decorView == null ) return result;

			var decorHeight = decorView.Call<int>( "getHeight" );
			result += decorHeight;

			return result;
		}
#else
        var area = TouchScreenKeyboard.area;
        var height = Mathf.RoundToInt(area.height);
        return Screen.height <= height ? 0 : height;
#endif
    }

}
