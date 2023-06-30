/*
 * 마지막으로 로그인한 날짜와 접속한 날짜의 Month가 다른 경우
 * -> 스탬프 초기화 O
 * 마지막 로그인한 날짜와 오늘 날짜의 Month가 같고 Day도 같은 경우
 * -> 스탬프 초기화 X
 * 마지막으로 로그인한 날짜와 오늘 날짜의 Month가 같고, Day가 다른 경우
 * -> 스탬프 초기화 O
 * 
 * 뒤끝 콘솔을 통해 테스트 진행
 */

using BackEnd;
using System;
using UnityEngine;

public class StampTourManager : MonoBehaviour
{
    private static StampTourManager instance;

    public static StampTourManager Instance
    {
        get { return instance; }
    }

    private int totalStamps = 5; // 총 스탬프 개수
    private int collectedStampsCount = 0; // 현재까지 모은 스탬프 개수
    private DateTime lastResetTime;
    bool[] collectedStamps;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        collectedStamps = new bool[totalStamps];
    }

    private void Init()
    {
        // 배열 내 모든 데이터를 false로 초기화
        collectedStamps.Initialize();
    }

    private void Start()
    {
        //1. 데이터의 마지막 로그인 정보 Get 
        //var userInfo = BackendReturnObject.Flatten(bro.Rows())[0];

        //2. 마지막 접속 기록과 현재 접속 날짜 비교
        //var lastLogin = userInfo["lastLogin"].ToString(); //있다고 가정
        //var lastLoginDate = DateTime.Parse(lastLogin);
        var lastLoginDate = DateTime.Now.AddDays(1).ToString("d");

        var nowDay = DateTime.Now.ToString("d");
        
        ServerDataInfo(lastLoginDate, nowDay);
    }

    /// <param name="myList">마지막 로그인 정보, 현재날짜</param>
    public void ServerDataInfo(params object[] myList)
    {
        //오늘 날짜의 Month와 마지막 로그인 날짜의 Month가 다를 경우 스탬프 북 초기화
        if (myList[1] != myList[0])
            Init();
        else //기존 스탬프 북 데이터 가져오기
        {

        }
    }

    public void CollectStamp(int enterNumCompelete)
    {
        collectedStampsCount = 0;
        collectedStamps[enterNumCompelete] = true;

        for (int i = 0; i < collectedStamps.Length; i++)
        {
            if (collectedStamps[i])
                collectedStampsCount++;
        }

        if (collectedStampsCount == totalStamps)
            ResetStamps(true);
    }

    private void ResetStamps(bool isPayment = false)
    {
        collectedStampsCount = 0;

        if (isPayment)
        {
            //모든 스탬프 북 획득
        }
        // 초기화 시간 갱신
        //lastResetTime = DateTime.Now;
    }

    //private void Update()
    //{
    //    // 매 프레임마다 현재 시간과 마지막 초기화 시간을 비교하여 24시가 경과했는지 확인
    //    if (DateTime.Now.Subtract(lastResetTime).TotalHours >= 24)
    //    {
    //        // 24시가 경과했을 때 변수 초기화
    //        ResetVariable();
    //    }
    //}

    //void ResetVariable()
    //{
       

    //    // 초기화 시간 갱신
    //    lastResetTime = DateTime.Now;
    //}
}
