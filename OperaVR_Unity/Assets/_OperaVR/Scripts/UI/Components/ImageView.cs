using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    public class ImageView : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private ResizeImageToContainer _imageResizer;

        [SerializeField]
        private Color _normalColor;

        [SerializeField]
        private Color _selectedColor;

        [SerializeField]
        private Color _hoveredColor;

        [SerializeField]
        private Color _heldColor;

        [SerializeField]
        private Color _hiddenColor;

        [SerializeField]
        private Color _notInteractableColor;

        [SerializeField]
        private Color _wrongColor;

        [SerializeField]
        private Color _correctColor;

        private Coroutine _coroutine;

        public void SetImageSprite(Sprite sprite)
        {
            _image.sprite = sprite;
            if (_imageResizer != null)
            {
                _imageResizer.Resize();
            }
        }

        public void SetSelected(bool isSelected)
        {
            var onStateColor = isSelected ? _selectedColor : _normalColor;
            UpdateView(onStateColor);
        }

        public void SetInteractable(bool isInteractable, bool isSelected)
        {
            var onStateColor = isSelected ? _selectedColor : _normalColor;
            var interactableStateColor = isInteractable ? onStateColor : _notInteractableColor;
            UpdateView(interactableStateColor);
        }

        public void SetHovered(bool isHovered, bool isSelected)
        {
            var onStateColor = isSelected ? _selectedColor : _normalColor;
            var hoveredStateColor = isHovered ? _hoveredColor : onStateColor;
            UpdateView(hoveredStateColor);
        }

        public void SetHeld(bool isHeld, bool isSelected)
        {
            var onStateColor = isSelected ? _selectedColor : _normalColor;
            var heldStateColor = isHeld ? _heldColor : onStateColor;
            UpdateView(heldStateColor);
        }

        public void SetHidden(bool isHidden, bool isSelected)
        {
            var onStateColor = isSelected ? _selectedColor : _normalColor;
            var hiddenStateColor = isHidden ? _hiddenColor : onStateColor;
            UpdateView(hiddenStateColor);
        }

        public void SetOutcome(bool isOutcomeGraphicOn, bool isCorrect)
        {
            if (!isOutcomeGraphicOn)
            {
                UpdateView(_normalColor);
                return;
            }
            UpdateView(isCorrect ? _correctColor : _wrongColor);
        }

        private void UpdateView(Color targetColor, float transitionTime = .2f)
        {
            if (!gameObject.activeInHierarchy)
            {
                _image.color = targetColor;
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
            var startingColor = _image.color;
            var t = 0f;
            while (t < transitionTime)
            {
                t += Time.deltaTime;
                var a = t / transitionTime;
                _image.color = Color.Lerp(startingColor, targetColor, a);
                yield return null;
            }
            _image.color = targetColor;
            _coroutine = null;
        }
    }
}
