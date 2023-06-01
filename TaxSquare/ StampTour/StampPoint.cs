public class StampPoint : MonoBehaviour
{
    private IStampState currentState;

    private void Start()
    {
        // 시작 상태로 초기화
        currentState = new StampState();
        currentState.Enter();
    }

    private void Update()
    {
        currentState.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 스탬프 지점에 도달하면 상태 변경
            currentState.Exit();
            currentState = new StampState();
            currentState.Enter();
        }
    }
}
