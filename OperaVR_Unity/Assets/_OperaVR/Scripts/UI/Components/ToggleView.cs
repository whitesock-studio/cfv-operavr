using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    public class ToggleView : MonoBehaviour
    {
        [SerializeField]
        private Image _onImage;
        
        [SerializeField]
        private Image _offImage;

        [SerializeField]
        private Color _normalColor;

        [SerializeField]
        private Color _hiddenColor;

        [SerializeField]
        private Color _notInteractableColor;

        private Coroutine _coroutine;

        public void SetOn(bool isOn)
        {
            _onImage.gameObject.SetActive(isOn);
            _offImage.gameObject.SetActive(!isOn);
            UpdateView(_normalColor);
        }

        public void SetInteractable(bool isInteractable, bool isSelected)
        {
            var interactableStateColor = isInteractable ? _normalColor : _notInteractableColor;
            UpdateView(interactableStateColor);
        }

        public void SetHidden(bool isHidden, bool isSelected)
        {
            var hiddenStateColor = isHidden ? _hiddenColor : _normalColor;
            UpdateView(hiddenStateColor);
        }

        private void UpdateView(Color targetColor, float transitionTime = .2f)
        {
            if (!gameObject.activeInHierarchy)
            {
                var image = _onImage.gameObject.activeInHierarchy ? _onImage : _offImage;
                image.color = targetColor;
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
            var image = _onImage.gameObject.activeInHierarchy ? _onImage : _offImage;
            var startingColor = image.color;
            var t = 0f;
            while (t < transitionTime)
            {
                t += Time.deltaTime;
                var a = t / transitionTime;
                image.color = Color.Lerp(startingColor, targetColor, a);
                yield return null;
            }
            image.color = targetColor;
            _coroutine = null;
        }
    }
}
