using System.Collections;
using TMPro;
using UnityEngine;

namespace OperaVR
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextView : MonoBehaviour
    {
        public TMP_Text TextDisplayer;

        [SerializeField]
        private Color _normalColor;

        [SerializeField]
        private Color _selectedColor;

        [SerializeField]
        private Color _notInteractableColor;

        [SerializeField]
        private Color _hiddenColor;

        [SerializeField]
        private Color _correctColor;

        [SerializeField]
        private Color _wrongColor;

        private Coroutine _coroutine;

        private void Awake()
        {
            UpdateView(_normalColor);
        }

        public void SetSelected(bool isSelected)
        {
            UpdateView(isSelected ? _selectedColor : _normalColor);
        }

        public void SetInteractable(bool isInteractable)
        {
            UpdateView(isInteractable ? _normalColor : _notInteractableColor);
        }

        public void SetHidden(bool isHidden)
        {
            UpdateView(isHidden ? _hiddenColor : _normalColor);
        }

        public void SetCorrect(bool isCorrect, bool isNormal)
        {
            if (isNormal)
            {
                TextDisplayer.color = _normalColor;
                return;
            }
            UpdateView(isCorrect ? _correctColor : _wrongColor);
        }

        private void UpdateView(Color targetColor, float transitionTime = .2f)
        {
            if (!gameObject.activeInHierarchy)
            {
                TextDisplayer.color = targetColor;
                return;
            }
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(UpdateViewCor(targetColor, transitionTime));
        }

        private IEnumerator UpdateViewCor(Color targetColor, float transitionTime = .2f)
        {
            var startingColor = TextDisplayer.color;
            var t = 0f;
            while (t < transitionTime)
            {
                t += Time.deltaTime;
                var a = t / transitionTime;
                TextDisplayer.color = Color.Lerp(startingColor, targetColor, a);
                yield return null;
            }
            TextDisplayer.color = targetColor;
            _coroutine = null;
        }
    }
}
