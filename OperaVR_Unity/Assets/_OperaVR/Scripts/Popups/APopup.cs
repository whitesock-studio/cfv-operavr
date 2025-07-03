using SpatialSys.UnitySDK;
using System.Collections;
using UnityEngine;

namespace OperaVR
{
    public abstract class APopup : MonoBehaviour
    {
        [ReadOnly]
        public APopupData Data;

        [Header("Open close animation")]
        [SerializeField]
        protected GameObject _hideableObject;

        [SerializeField]
        protected Transform _animatedObject;

        [SerializeField]
        private AnimationCurve _openCurveX;

        [SerializeField]
        private AnimationCurve _openCurveY;

        [SerializeField]
        private float _openTime = 1f;

        private Coroutine _animationCoroutine;

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
        }

        protected virtual void Reset() { }

        protected virtual void OnPreOpened() { }

        protected virtual void OnPostOpened() { }

        protected virtual void OnPreClosed() { }

        protected virtual void OnPostClosed() { }

        private IEnumerator OpenCloseCoroutine(bool isClosing)
        {
            _hideableObject.SetActive(false);

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

            _hideableObject.SetActive(!isClosing);

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