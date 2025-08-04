using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OperaVR
{
    public class CustomImageSlider : MonoBehaviour, IPointerDownHandler
    {
        public Action<float> OnNormalizedTimeRequest;

        [SerializeField]
        private RectTransform _rectTransform;

        [SerializeField]
        private RectTransform _maskTransform;

        public void SetNormalizedValue(float normalizedValue)
        {
            if (_maskTransform == null)
            {
                return;
            }
            _maskTransform.anchorMax = new Vector2(normalizedValue, 1);
            _maskTransform.offsetMax = Vector2.zero;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform, eventData.position, eventData.pressEventCamera, out var localPosition))
            {
                return;
            }

            var normalizedPosition = Rect.PointToNormalized(_rectTransform.rect, localPosition);

            OnNormalizedTimeRequest?.Invoke(normalizedPosition.x);
        }
    }
}
