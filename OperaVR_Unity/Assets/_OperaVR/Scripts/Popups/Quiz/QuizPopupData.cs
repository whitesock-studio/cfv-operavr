using System;
using UnityEngine;

namespace OperaVR
{
    [CreateAssetMenu(menuName = "Popup/Data/Quiz", fileName = "QuizData_")]
    public class QuizPopupData : APopupData
    {
        [Header("Quiz Popup")]
        public QuizType Type;
        public bool CanGoBack => Type == QuizType.ProfilingQuiz;
        public QuizQuestion[] Questions = Array.Empty<QuizQuestion>();
    }

    public enum QuizType
    {
        LinearQuiz,
        ProfilingQuiz
    }

    [Serializable]
    public class QuizQuestion
    {
        public Sprite Image;
        public QuizContent Content;
        public string CorrectChoiceDescription = "Questa è la risposta giusta, complimenti!"; //TODO: LOCALIZE

        public int MaxChoices = 1;

        public QuizChoice[] Choices;
    }

    [Serializable]
    public class QuizChoice
    {
        public bool IsCorrect;
        public int PointsGiven;
        public QuizContent Content;
    }
}