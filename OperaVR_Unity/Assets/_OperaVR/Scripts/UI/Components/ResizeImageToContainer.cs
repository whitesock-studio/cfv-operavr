using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    public class ResizeImageToContainer : MonoBehaviour
    {
        public enum ResizeMode
        {
            Stretch,
            BestFitInside,
            BestFitOutside
        }

        [SerializeField]
        private ResizeMode _resizeMode;

        [SerializeField]
        private bool _resizeAtStart;

        [SerializeField]
        private bool _resizeInEditor;

        [SerializeField]
        private bool _resizeInUpdate;

        private Image m_image;
        private Image Image
        {
            get
            {
                if (m_image == null || !Application.isPlaying)
                {
                    m_image = GetComponent<Image>();
                }
                return m_image;
            }
        }

        private RectTransform m_container;
        private RectTransform Container
        {
            get
            {
                if (m_container == null || !Application.isPlaying)
                {
                    m_container = transform.parent.GetComponent<RectTransform>(); 
                }
                return m_container;
            }
        }

        private void OnValidate()
        {
            if (!_resizeInEditor || !Image || !Container)
            {
                return;
            }
            Resize();
        }

        private void Start()
        {
            if (!_resizeAtStart || !Image || !Container)
            {
                return;
            }
            Resize();
        }

        private void Update()
        {
            if (!_resizeInUpdate || !Image || !Container)
            {
                return;
            }
            Resize();
        }

        public void Resize()
        {
            if (!Image || !Container)
            {
                return;
            }
            switch(_resizeMode)
            {
                case ResizeMode.Stretch:
                    Stretch();
                    return;
                case ResizeMode.BestFitInside:
                    BestFit(true);
                    return;
                case ResizeMode.BestFitOutside:
                    BestFit(false);
                    return;
            }
        }

        private void Stretch()
        {
            Image.preserveAspect = false;
            Image.rectTransform.anchorMin = Vector2.zero;
            Image.rectTransform.anchorMax = Vector2.one;
            Image.rectTransform.offsetMin = Vector2.zero;
            Image.rectTransform.offsetMax = Vector2.zero;
        }

        private void BestFit(bool inside)
        {
            if (Image.sprite == null)
            {
                return;
            }
            Image.preserveAspect = true;
            Image.rectTransform.anchorMin = Vector2.one / 2;
            Image.rectTransform.anchorMax = Vector2.one / 2;
            var textureSize = Image.sprite.textureRect.size;
            var containersize = Container.rect.size;
            var fitSize = textureSize;
            var horizontalRatio = textureSize.x / containersize.x;
            var verticalRatio = textureSize.y / containersize.y;
            var leadingRatio = inside ? 
                Mathf.Max(horizontalRatio, verticalRatio) : 
                Mathf.Min(horizontalRatio, verticalRatio);
            var multiplier = 1 / leadingRatio;
            fitSize = textureSize * multiplier;

            Image.rectTransform.sizeDelta = fitSize; 
        }
    }
}
