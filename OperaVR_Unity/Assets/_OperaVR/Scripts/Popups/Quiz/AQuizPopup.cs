using UnityEngine;

namespace OperaVR
{
    public abstract class AQuizPopup : APopup
    {
        [SerializeField]
        protected QuizContentController ContentController;  

        public QuizPopupData QuizData => Data as QuizPopupData;

        protected override void OnPreOpened()
        {
            base.OnPreOpened();
            ContentController.LoadData(QuizData);
        }
    }
}