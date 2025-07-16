using UnityEngine;

namespace OperaVR
{
    public class QuizChoiceToggle : OperaToggle
    {
        [SerializeField]
        protected ToggleView AlternativeToggleView;

        [SerializeField]
        private QuizContentView _quizContentView;

        [Header("Outcome")]
        [SerializeField]
        private GameObject _correctGroup;
        
        [SerializeField]
        private GameObject _correctIcon;

        [SerializeField]
        private GameObject _wrongGroup;

        [SerializeField]
        private GameObject _wrongIcon;

        private QuizChoice _loadedChoice;
        public QuizChoice LoadedChoice => _loadedChoice;

        public override bool IsOn
        {
            get => base.IsOn;
            protected set
            {
                base.IsOn = value;
                _quizContentView.SetSelected(base.IsOn);
                AlternativeToggleView?.SetOn(IsOn);
            }
        }

        public override bool IsInteractable
        {
            get => base.IsInteractable;
            protected set
            {
                base.IsInteractable = value;
                _quizContentView.SetInteractable(base.IsInteractable, IsOn);
                AlternativeToggleView?.SetInteractable(IsInteractable, IsOn);
            }
        }

        public override bool IsHidden
        {
            get => base.IsHidden;
            set
            {
                base.IsHidden = value;
                _quizContentView.SetHidden(base.IsHidden, IsOn);
                AlternativeToggleView?.SetHidden(IsHidden, IsOn);
            }
        }

        public override void Reset()
        {
            base.Reset();
            HideCorrectState();
        }

        public void LoadChoice(QuizChoice quizChoice, bool isSingleChoice)
        {
            Reset();
            _loadedChoice = quizChoice;
            ToggleView.gameObject.SetActive(isSingleChoice);
            AlternativeToggleView.gameObject.SetActive(!isSingleChoice);
            _quizContentView.LoadContent(_loadedChoice.Content);
        }

        public void HideCorrectState()
        {
            _correctGroup.SetActive(true);
            _correctIcon.SetActive(false);
            _wrongGroup.SetActive(false);
            _wrongIcon.SetActive(false);
            BorderView.SetOutcome(false, false);
        }

        public void DisplayCorrectState(bool isCorrect)
        {
            _correctGroup.SetActive(isCorrect);
            _correctIcon.SetActive(isCorrect);
            _wrongGroup.SetActive(!isCorrect);
            _wrongIcon.SetActive(!isCorrect);
            BorderView.SetOutcome(true, isCorrect);
        }
    }
}
