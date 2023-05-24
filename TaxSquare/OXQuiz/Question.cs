/*
 * desc   : 퀴즈의 문제와 정답 데이터
 * author : DX팀 김민성
 * since  : 2023.05.24
 */
using UnityEngine;

public class Question
{
    public string QuestionText { get; private set; }
    public bool IsCorrect { get; private set; }

    public Question(string questionText, bool isCorrect)
    {
        QuestionText = questionText;
        IsCorrect = isCorrect;
    }
}
