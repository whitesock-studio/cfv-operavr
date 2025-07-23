using UnityEngine;

namespace OperaVR
{
    public abstract class AQuizPopup : APopup
    {
        [SerializeField]
        protected QuizContentController ContentController;  

        public QuizPopupData QuizData => Data as QuizPopupData;

        protected virtual void Awake()
        {
            ContentController.OnComplete += OnQuizCompleted;
            ContentController.OnRestart += OnContentRestart;
        }

        protected override void OnPreOpened()
        {
            base.OnPreOpened();
            ContentController.LoadData(QuizData);
        }

        protected virtual void OnQuizCompleted() { }

        private void OnContentRestart()
        {
            ContentController.ChoicesSelected = new();
        }
    }
}