using System.Collections;
using UnityEngine;

namespace OperaVR
{
    public class CodelockSlot : MonoBehaviour
    {
        [SerializeField]
        private TextView _textView;

        private Coroutine _coroutine;

        public void Reset()
        {
            _textView.TextDisplayer.text = string.Empty;
            _textView.SetSelected(true);
        }

        public void SetText(string text)
        {
            _textView.TextDisplayer.text = text;
        }

        public void PulseOutcome(bool isCorrect)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(Pulse(isCorrect));
        }

        private IEnumerator Pulse(bool isCorrect)
        {
            var holdT = 1.5f;
            _textView.SetCorrect(isCorrect, true);
            yield return new WaitForSeconds(holdT);
            _textView.SetCorrect(isCorrect, false);
            _coroutine = null;
        }
    }
}
