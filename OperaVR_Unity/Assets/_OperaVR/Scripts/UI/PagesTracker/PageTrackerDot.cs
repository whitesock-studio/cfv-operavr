using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    public class PageTrackerDot : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private float _onScaleMultiplier = 1f;
        
        [SerializeField]
        private float _offScaleMultiplier = .7f;

        [SerializeField]
        private float _onAlpha = 1f;

        [SerializeField]
        private float _offAlpha = .5f;

        public void SetOn(bool value)
        {
            var scale = value ? _onScaleMultiplier : _offScaleMultiplier;
            _image.transform.localScale = new Vector3(scale, scale, 1f);

            var color = _image.color;
            color.a = value ? _onAlpha : _offAlpha;
            _image.color = color;
        }
    }
}
