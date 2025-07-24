using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    public class QuizContentController : MonoBehaviour
    {
        public Action OnComplete;
        public Action OnRestart;
        public Action OnFail;
        public Action<List<QuizChoice>> OnChoicesSelected;

        [NonSerialized]
        public List<List<QuizChoice>> ChoicesSelected = new();

        [SerializeField]
        private QuizChoicesController _choicesController;

        [SerializeField]
        private LayoutElement _choicesLayoutElement;

        [SerializeField]
        private LayoutGroup _questionsAndChoicesLayoutGroup;

        [SerializeField]
        private QuizContentView _questionContentView;

        [SerializeField]
        private Image _image;

        [SerializeField]
        private GameObject _imageContainer;

        [SerializeField]
        private TMP_Text _outcomeComment;

        [Header("Controls")]
        [SerializeField]
        private Button _confirmButton;

        [SerializeField]
        private TextView _confirmButtonText;
        protected TextView ConfirmButtonText => _confirmButtonText;

        [SerializeField]
        private Button _backButton;

        [SerializeField]
        private TextView _backButtonText;

        [SerializeField]
        private Button _restartButton;

        [SerializeField]
        private Button _confirmOutcomeButton;

        [SerializeField]
        private TextView _confirmOutcomeButtonText;

        [SerializeField]
        private APagesTracker _pagesTracker;

        private int _displayedPage = 0;
        private QuizQuestion _currentQuestion;
        private QuizPopupData _quizPopupData;

        private void Awake()
        {
            _choicesController.OnTogglesChanged += OnTogglesSelectionChanged;
            _confirmButton.onClick.AddListener(Confirm);
            _confirmOutcomeButton.onClick.AddListener(Progress);
            _restartButton.onClick.AddListener(Restart);
            _backButton.onClick.AddListener(Back);
        }

        public void LoadData(QuizPopupData data)
        {
            _quizPopupData = data;
            _backButton.transform.gameObject.SetActive(_quizPopupData.CanGoBack);
            LoadQuestionPage(0);
        }

        protected virtual void LoadQuestionPage(int pageIndex)
        { 
            if (_quizPopupData.Questions.Length <= pageIndex)
            {
                Debug.LogError("Page index exceed Questions number");
                return;
            }
            _displayedPage = pageIndex;

            _choicesLayoutElement.flexibleHeight = 1;
            _questionsAndChoicesLayoutGroup.childAlignment = TextAnchor.UpperLeft;

            _confirmOutcomeButton.gameObject.SetActive(false);
            _restartButton.gameObject.SetActive(false);
            _confirmButton.gameObject.SetActive(true);

            //TODO: LOCALIZE
            _confirmButtonText.TextDisplayer.text = _quizPopupData.Type == QuizType.LinearQuiz ? "Avanti" :
                _quizPopupData.Questions.Length == pageIndex + 1 ? "Vedi risultati" : "Avanti"; 
            _confirmOutcomeButtonText.TextDisplayer.text = 
                _quizPopupData.Questions.Length == pageIndex + 1 ? "Chiudi" : "Prossima domanda";

            CheckControlButtonsInteractability(_choicesController.TogglesOn);

            _outcomeComment.gameObject.SetActive(false);

            _currentQuestion = _quizPopupData.Questions[pageIndex];

            _questionContentView.LoadContent(_currentQuestion.Content);

            _image.sprite = _currentQuestion.Image;
            _imageContainer.SetActive(_currentQuestion.Image != null);

            var previouslySelectedChoices = ChoicesSelected.Count > _displayedPage ?
                ChoicesSelected[_displayedPage] : new();
            _choicesController.LoadChoices(_currentQuestion.Choices, _currentQuestion.MaxChoices, previouslySelectedChoices);

            SetPage(pageIndex);
        }

        private void CheckControlButtonsInteractability(QuizChoiceToggle[] togglesOn)
        {
            _backButton.interactable = _displayedPage > 0;
            _backButtonText.SetInteractable(_displayedPage > 0);

            var canConfirm = togglesOn.Length > 0;
            _confirmButton.interactable = canConfirm;
        }

        private void OnTogglesSelectionChanged()
        {
            var togglesOn = _choicesController.TogglesOn;
            CheckControlButtonsInteractability(togglesOn);
        }

        private void SetPage(int pageIndex)
        {
            _pagesTracker.UpdateTracker(_quizPopupData.Questions.Length, pageIndex);
        }

        private void Confirm()
        {
            ConfirmChoices();

            if (_quizPopupData.Type == QuizType.LinearQuiz)
            {
                DisplayChoiceOutcome();
                return;
            }
            Progress();
        }

        private void ConfirmChoices()
        {
            var choicesSelected = new List<QuizChoice>();
            foreach (var toggle in _choicesController.TogglesOn)
            {
                choicesSelected.Add(toggle.LoadedChoice);
            }
            if (ChoicesSelected.Count > _displayedPage)
            {
                ChoicesSelected[_displayedPage] = choicesSelected;
            }
            else
            {
                ChoicesSelected.Add(choicesSelected);
            }
            OnChoicesSelected?.Invoke(choicesSelected);
        }

        private void DisplayChoiceOutcome()
        {
            var isCorrectChoice = true;
            foreach (var toggle in _choicesController.TogglesOn)
            {
                if (!toggle.LoadedChoice.IsCorrect)
                {
                    isCorrectChoice = false;
                }
            }

            _choicesController.DisplaySelectedChoicesOutcome();

            _choicesLayoutElement.flexibleHeight = 0;
            _questionsAndChoicesLayoutGroup.childAlignment = TextAnchor.MiddleLeft;

            _confirmButton.gameObject.SetActive(false);
            _confirmOutcomeButton.gameObject.SetActive(isCorrectChoice);
            _restartButton.gameObject.SetActive(!isCorrectChoice);

            if (!isCorrectChoice)
            {
                OnFail?.Invoke();
            }

            //TODO: LOCALIZE
            var outcomeText = isCorrectChoice ? _currentQuestion.CorrectChoiceDescription : "Risposta errata, ricomincia";
            _outcomeComment.text = outcomeText;
            _outcomeComment.gameObject.SetActive(true);
        }

        private void Progress()
        {
            if (_quizPopupData.Questions.Length <= _displayedPage + 1)
            {
                QuizCompleted();
                return;
            }
            
            LoadQuestionPage(_displayedPage + 1);
        }

        private void Back()
        {
            LoadQuestionPage(_displayedPage - 1);
        }

        private void Restart()
        {
            LoadQuestionPage(0);
            OnRestart?.Invoke();
        }

        private void QuizCompleted()
        {
            OnComplete?.Invoke();
        }
    }
}
