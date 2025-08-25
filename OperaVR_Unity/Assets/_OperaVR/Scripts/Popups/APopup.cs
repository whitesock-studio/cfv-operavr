using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace OperaVR
{
    public abstract class APopup : MonoBehaviour
    {
        public Action<APopup> OnClose;
        public Action<APopup> OnSuccess;
        public Action<APopup> OnFail;

        public APopupData Data;

        [Header("Open close animation")]
        [SerializeField]
        protected CanvasGroup _hideableCanvas;

        [SerializeField]
        protected Transform _animatedObject;

        [SerializeField]
        private AnimationCurve _openCurveX;

        [SerializeField]
        private AnimationCurve _openCurveY;

        [SerializeField]
        private float _openTime = 1f;

        private Coroutine _animationCoroutine;

        [Header("Components")]
        [SerializeField]
        protected LocalizedText _titleLoc;

        [SerializeField]
        protected LocalizedText _subTitleLoc;

        public abstract bool IsComplete { get; }

        public virtual void Open(APopupData data)
        {
            data = Data;
            OnPreOpened();
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            _animationCoroutine = StartCoroutine(OpenCloseCoroutine(false));
        }

        public virtual void Close()
        {
            OnPreClosed();
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            _animationCoroutine = StartCoroutine(OpenCloseCoroutine(true));
            OnClose?.Invoke(this);
        }

        protected virtual void Reset() { }

        protected virtual void OnPreOpened() 
        {
            _titleLoc.Key = Data.TitleKey;
            _subTitleLoc.Key = Data.SubTitleKey;
        }

        protected virtual void OnPostOpened() { }

        protected virtual void OnPreClosed() { }

        protected virtual void OnPostClosed() { }

        private IEnumerator OpenCloseCoroutine(bool isClosing)
        {
            _hideableCanvas.alpha = 0f;

            var t = 0f;
            while (t < _openTime)
            {
                t = Mathf.Clamp(t + Time.deltaTime, 0, _openTime);
                var a = t / _openTime;
                a = isClosing ? 1 - a : a;
                _animatedObject.localScale =
                    new Vector3(_openCurveX.Evaluate(a), _openCurveY.Evaluate(a), 1);

                yield return null;
            }

            t = 0f;
            var fadeTime = .3f;
            var targetAlpha = isClosing ? 0f : 1f;
            while (t < fadeTime)
            {
                t = Mathf.Clamp(t + Time.deltaTime, 0, fadeTime);
                var a = t / _openTime;
                _hideableCanvas.alpha = Mathf.Lerp(0f, targetAlpha, a);
                yield return null;
            }
            _hideableCanvas.alpha = targetAlpha;

            if (isClosing)
            {
                OnPostClosed();
            }
            else
            {
                OnPostOpened();
            }
            _animationCoroutine = null;
        }
    }
}