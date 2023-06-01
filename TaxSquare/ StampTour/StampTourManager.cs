public class StampTourManager : MonoBehaviour
{
    private static StampTourManager instance;

    public static StampTourManager Instance
    {
        get { return instance; }
    }

    private int totalStamps = 5; // 총 스탬프 개수
    private int collectedStamps = 0; // 현재까지 모은 스탬프 개수

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 스탬프 개수 초기화
        collectedStamps = 0;
    }

    public void CollectStamp()
    {
        collectedStamps++;

        if (collectedStamps == totalStamps)
        {
            // 모든 스탬프를 모았을 때 포인트 지급 등의 로직 실행
            ResetStamps();
        }
    }

    private void ResetStamps()
    {
        // 매일 자정 12시에 호출되어 스탬프 초기화
        collectedStamps = 0;
    }
}
