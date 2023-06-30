using UnityEngine;

public class StampState : IStampState
{
    private float waitTime = 5f; // 대기 시간 설정
    private float elapsedTime = 0f; // 경과 시간
    int enterNum = 0;
    /// <summary>
    /// Player가 지정된 영역에 진입 시
    /// </summary>
    public void Enter(int areaNum)
    {
        // 상태 진입 시 실행할 내용
        Debug.Log("지정된 영역에 들어왔습니다. ");
        enterNum = areaNum;
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= waitTime)
        {
            Debug.Log("타이머 " + elapsedTime);
            Exit(true);
        }
    }

    public void Exit(bool isCompelete = false)
    {
        if (!isCompelete)
        {
            Debug.Log("영역을 벗어남 : " + elapsedTime);

        }
        
        elapsedTime = 0f;
        if (isCompelete) 
            StampTourManager.Instance.CollectStamp(enterNum);
    }
}
