using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    public class CodelockSlot : MonoBehaviour
    {
        [SerializeField]
        private TextView _textView;

        [SerializeField]
        private Image _background;

        [SerializeField]
        private Color _normalColor;
        
        [SerializeField]
        private Color _editingColor;

        private Coroutine _coroutine;

        private void Awake()
        {
            SetIsEditing(false);
        }

        public void Reset()
        {
            _textView.TextDisplayer.text = string.Empty;
            _textView.SetSelected(false);
        }

        public void SetText(string text)
        {
            _textView.TextDisplayer.text = text;
        }

        public void SetIsEditing(bool value)
        {
            _background.color = value ? _editingColor : _normalColor;
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
            _textView.SetCorrect(isCorrect, false);
            yield return new WaitForSeconds(holdT);
            _textView.SetCorrect(isCorrect, true);
            _coroutine = null;
        }
    }
}
