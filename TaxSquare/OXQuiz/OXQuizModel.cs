/*
 * desc   : 게임 데이터와 로직 처리 (OX 퀴즈의 문제와 정답 데이터, 게임 진행 상태)
 * author : DX팀 김민성
 * since  : 2023.05.24
 */
using System.Collections.Generic;
using UnityEngine;

public class OXQuizModel : MonoBehaviour
{
    private List<Question> questions;
    private int currentIndex;

    public OXQuizModel()
    {
        questions = new List<Question>();
        currentIndex = 0;

        InitializeQuestions();
    }

    /// <summary>
    /// 퀴즈 문제와 정답을 초기화
    /// </summary>
    private void InitializeQuestions()
    {
        // 퀴즈 문제와 정답을 추가합니다.
        // questions 리스트에 Question 객체들을 추가하는 로직을 작성합니다.

        questions.Add(new Question("세금 관련 질문 예제 1", true));
        questions.Add(new Question("세금 관련 질문 예제 2", false));
        // 추가적인 퀴즈 문제와 정답을 여기에 추가합니다.
    }

    public Question GetCurrentQuestion()
    {
        if (currentIndex < questions.Count)
            return questions[currentIndex];
        else
            return null;
    }

    public bool CheckAnswer(bool isCorrect)
    {
        bool result = GetCurrentQuestion().IsCorrect == isCorrect;
        // 정답 여부에 따른 처리 로직을 작성합니다.
        // 예를 들어, 점수를 증가시키거나 다음 문제로 넘어가는 동작을 수행할 수 있습니다.
        return result;
    }

    public void GoToNextQuestion()
    {
        currentIndex++;
        // 다음 문제로 이동하는 로직을 작성합니다.
    }
   
}
