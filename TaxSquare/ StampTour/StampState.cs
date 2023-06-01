public class StampState : IStampState
{
    private float waitTime = 5f; // 대기 시간 설정
    private float elapsedTime = 0f; // 경과 시간

    public void Enter()
    {
        // 상태 진입 시 실행할 내용
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= waitTime)
        {
            // 스탬프 찍기 로직
            // 스탬프 찍힌 후 다음 상태로 전환
            Exit();
        }
    }

    public void Exit()
    {
        // 상태 종료 시 실행할 내용
    }
}
