/*
 * desc   : 사용자 입력을 처리하고 모델을 업데이트하여 게임 상태를 관리
 * author : DX팀 김민성
 * since  : 2023.05.24
 */
using UnityEngine;

public class OXQuizController : MonoBehaviour
{
    private OXQuizModel model;
    private OXQuizView view;

    private void Start()
    {
        model = new OXQuizModel();
        view = FindObjectOfType<OXQuizView>();
    }

    public Question GetCurrentQuestion()
    {
        return model.GetCurrentQuestion();
    }

    public bool CheckAnswer(bool isCorrect)
    {
        return model.CheckAnswer(isCorrect);
    }

    public void GoToNextQuestion()
    {
        model.GoToNextQuestion();
    }
}
