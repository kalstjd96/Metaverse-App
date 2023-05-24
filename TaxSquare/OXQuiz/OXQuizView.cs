/*
 * desc   : 질문과 선택지를 표시하고 사용자 입력을 처리하는 역할
 * author : DX팀 김민성
 * since  : 2023.05.24
 */
using UnityEngine;
using UnityEngine.UI;

public class OXQuizView : MonoBehaviour
{
    private OXQuizController controller;
    public Text questionText;
    public Button trueButton;
    public Button falseButton;

    private void Start()
    {
        controller = FindObjectOfType<OXQuizController>();
        UpdateUI();
    }

    private void UpdateUI()
    {
        Question currentQuestion = controller.GetCurrentQuestion();
        if (currentQuestion != null)
        {
            questionText.text = currentQuestion.QuestionText;
        }
        else
        {
            questionText.text = "퀴즈가 종료되었습니다.";
            trueButton.interactable = false;
            falseButton.interactable = false;
        }
    }

    public void OnAnswerSelected(bool isCorrect)
    {
        bool result = controller.CheckAnswer(isCorrect);
        // 정답 여부에 따른 처리 로직을 작성합니다.
        // 예를 들어, 정답 결과를 화면에 표시하거나 게임 종료를 처리할 수 있습니다.
        controller.GoToNextQuestion();
        UpdateUI();
    }
}
