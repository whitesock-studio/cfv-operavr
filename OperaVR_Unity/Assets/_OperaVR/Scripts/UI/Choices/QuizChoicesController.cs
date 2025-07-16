using System;
using System.Linq;
using UnityEngine;

namespace OperaVR
{
    public class QuizChoicesController : MonoBehaviour
    {
        public Action OnTogglesChanged;

        [SerializeField]
        private QuizChoiceToggle _outcomeToggle;

        [SerializeField]
        private OperaToggleGroup _toggleGroup;

        private QuizChoiceToggle[] _pool;

        public QuizChoiceToggle[] TogglesOn => _pool.Where(t => t.IsOn && t.gameObject.activeInHierarchy).ToArray();

        private void Awake()
        {
            _pool = GetComponentsInChildren<QuizChoiceToggle>();
            foreach (var toggle in _pool)
            {
                toggle.OnValueChanged += (_, _) => OnTogglesChanged?.Invoke();
            }
        }

        public void LoadChoices(QuizChoice[] quizChoices, int maxChoices)
        {
            _outcomeToggle.gameObject.SetActive(false);

            _toggleGroup.AlwaysOneOn = false;
            _toggleGroup.MaxTogglesOn = maxChoices;
            _toggleGroup.SwitchAtMax = maxChoices == 1;
            _toggleGroup.SetNotInteractableOnMaxReached = true;

            var current = 0;
            for (var i = 0; i < quizChoices.Length; i++)
            {
                if (_pool.Length <= i)
                {
                    continue;
                }
                var quizChoiceToggle = _pool[i];
                quizChoiceToggle.gameObject.SetActive(true);
                quizChoiceToggle.LoadChoice(quizChoices[i], maxChoices == 1);
                current++;
            }
            for (var i = current; i < _pool.Length; i++)
            {
                var quizChoiceToggle = _pool[i];
                quizChoiceToggle.gameObject.SetActive(false);
            }
        }

        public void DisplaySelectedChoicesOutcome()
        {
            //We assume that this happens only in linear quiz
            foreach (var toggle in _pool)
            {
                toggle.gameObject.SetActive(false);
                if (toggle.IsOn)
                {
                    _outcomeToggle.gameObject.SetActive(true);
                    _outcomeToggle.LoadChoice(toggle.LoadedChoice, toggle.IsSingleChoice);
                    _outcomeToggle.SetInteractable(false);
                    _outcomeToggle.DisplayCorrectState(toggle.LoadedChoice.IsCorrect);
                    continue;
                }
            }
        }
    }
}
