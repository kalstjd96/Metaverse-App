using Unity.VisualScripting;
using UnityEngine;

public class StampPoint : MonoBehaviour
{
    private IStampState currentState;
    [SerializeField] private int pointNum;
    bool areaEnter = false;

    private void Start()
    {
        // 시작 상태로 초기화
        currentState = new StampState();
    }

    /// <summary>
    /// Player가 지정된 영역에 들어왔을 때
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        //currentState.Exit();
        //currentState = new StampState();
        currentState.Enter(this.pointNum);
        areaEnter = true;
        #region 삭제 예정
        //if (other.CompareTag("Player"))
        //{
        //    // 플레이어가 스탬프 지점에 도달하면 상태 변경
        //    currentState.Exit();
        //    currentState = new StampState();
        //    currentState.Enter();
        //}
        #endregion
    }

    private void OnTriggerStay(Collider other)
    {
        if (areaEnter)
        {
            currentState.Update();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            areaEnter = false;
            currentState.Exit();
        }
    }
}
