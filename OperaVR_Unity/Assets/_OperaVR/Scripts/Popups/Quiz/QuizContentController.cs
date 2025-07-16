using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    public class QuizContentController : MonoBehaviour
    {
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

        [SerializeField]
        private Button _backButton;

        [SerializeField]
        private TextView _backButtonText;

        [SerializeField]
        private APagesTracker _pagesTracker;

        private int _displayedPage = 0;
        private QuizQuestion _currentQuestion;
        private QuizPopupData _quizPopupData;

        private void Awake()
        {
            _choicesController.OnTogglesChanged += OnTogglesSelectionChanged;
            _confirmButton.onClick.AddListener(Confirm);
            _backButton.onClick.AddListener(Back);
        }

        public void LoadData(QuizPopupData data)
        {
            _quizPopupData = data;
            _backButton.transform.parent.gameObject.SetActive(_quizPopupData.CanGoBack);
            LoadQuestionPage(0);
        }

        private void LoadQuestionPage(int pageIndex)
        {
            if (_quizPopupData.Questions.Length <= pageIndex)
            {
                Debug.LogError("Page index exceed Questions number");
                return;
            }
            _displayedPage = pageIndex;

            _choicesLayoutElement.flexibleHeight = 1;
            _questionsAndChoicesLayoutGroup.childAlignment = TextAnchor.UpperLeft;

            CheckControlButtonsInteractability(_choicesController.TogglesOn);

            _outcomeComment.gameObject.SetActive(false);

            _currentQuestion = _quizPopupData.Questions[pageIndex];

            _questionContentView.LoadContent(_currentQuestion.Content);

            _image.sprite = _currentQuestion.Image;
            _imageContainer.SetActive(_currentQuestion.Image != null);

            _confirmButtonText.TextDisplayer.text =
                _quizPopupData.Questions.Length == pageIndex + 1 ? "Vedi risultati" : "Avanti"; //TODO: LOCALIZE

            _choicesController.LoadChoices(_currentQuestion.Choices, _currentQuestion.MaxChoices);

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
            _confirmButton.onClick.RemoveListener(Confirm);

            if (_quizPopupData.Type == QuizType.LinearQuiz)
            {
                DisplayChoiceOutcome();
                return;
            }
            Progress();
        }

        private void DisplayChoiceOutcome()
        {
            _confirmButton.onClick.AddListener(Progress);

            _choicesController.DisplaySelectedChoicesOutcome();

            _choicesLayoutElement.flexibleHeight = 0;
            _questionsAndChoicesLayoutGroup.childAlignment = TextAnchor.MiddleLeft;

            var isCorrectChoice = true;
            foreach (var toggle in _choicesController.TogglesOn)
            {
                if (!toggle.LoadedChoice.IsCorrect)
                {
                    isCorrectChoice = false;
                }
            }

            //TODO: LOCALIZE
            var confirmButtonText = isCorrectChoice ? "Prossima domanda" : "Ricomincia Quiz";
            _confirmButtonText.TextDisplayer.text = confirmButtonText;

            //TODO: LOCALIZE
            var outcomeText = isCorrectChoice ? _currentQuestion.CorrectChoiceDescription : "Risposta errata, ricomincia";
            _outcomeComment.text = outcomeText;
            _outcomeComment.gameObject.SetActive(true);
        }

        private void Progress()
        {
            _confirmButton.onClick.RemoveListener(Progress);

            if (_quizPopupData.Questions.Length <= _displayedPage + 1)
            {
                QuizCompleted();
                return;
            }
            
            _confirmButton.onClick.AddListener(Confirm);
            LoadQuestionPage(_displayedPage + 1);
        }

        private void Back()
        {
            LoadQuestionPage(_displayedPage - 1);
        }

        private void QuizCompleted()
        {

        }
    }
}
