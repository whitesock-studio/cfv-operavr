using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    public class SliderPagesTracker : APagesTracker
    {
        [SerializeField]
        private TMP_Text _textDisplayer;

        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private string _defaultText = "Domanda"; //TODO: LOCALIZE

        private Coroutine _updateCoroutine;

        public override void UpdateTracker(int pagesNumber, int selectedPageIndex)
        {
            _textDisplayer.text = $"{_defaultText} {selectedPageIndex + 1}/{pagesNumber}";
            if (!gameObject.activeInHierarchy)
            {
                _slider.value = (float)(selectedPageIndex + 1) / pagesNumber;
                return;
            }
            if (_updateCoroutine != null)
            {
                StopCoroutine(_updateCoroutine);
            }
            _updateCoroutine = StartCoroutine(UpdateTrackerCor((float)(selectedPageIndex + 1) / pagesNumber));
        }

        private IEnumerator UpdateTrackerCor(float targetValue)
        {
            var t = 0f;
            var maxT = .5f;
            var startingValue = _slider.value;
            while(t < maxT)
            {
                t += Time.deltaTime;
                _slider.value = Mathf.Lerp(startingValue, targetValue, t / maxT);
                yield return null;
            }
            _slider.value = targetValue;
            _updateCoroutine = null;
        }
    }
}
