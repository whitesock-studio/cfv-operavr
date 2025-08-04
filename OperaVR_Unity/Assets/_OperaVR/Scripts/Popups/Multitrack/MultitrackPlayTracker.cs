using System;
using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    public class MultitrackPlayTracker : MonoBehaviour
    {
        public Action<float> OnValueChanged;

        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private CustomImageSlider _imageSlider;

        private void Awake()
        {
            _slider.onValueChanged.AddListener(OnSliderDragged);
            _imageSlider.OnNormalizedTimeRequest += (normValue) => _slider.value = normValue;
        }

        public void SetValue(float normalizedValue)
        {
            _slider.SetValueWithoutNotify(normalizedValue);
        }

        private void OnSliderDragged(float normalizedValue)
        {
            OnValueChanged?.Invoke(Mathf.Clamp01(normalizedValue));
        }
    }
}
