using System.Collections;
using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField]
        private GameObject _defaultGroup;

        [SerializeField]
        private GameObject _metaQuestGroup;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Transform _invertedSphere;
        
        [SerializeField]
        private MeshRenderer _meshRenderer;

        private MaterialPropertyBlock _mpb;
        
        private float _alpha = 0f;
        private Coroutine _fadeCoroutine;
        
        private const string COLOR_KEY = "_BaseColor";

        private void Awake()
        {
            _mpb ??= new MaterialPropertyBlock();
            _alpha = 0f;
            SetAlpha();
            
            _defaultGroup.SetActive(false);
            _metaQuestGroup.SetActive(false);
        }

        private void Update()
        {
            if (_invertedSphere.gameObject.activeInHierarchy)
            {
                var mainCamera = Camera.main;
                _invertedSphere.transform.position = mainCamera.transform.position;
            }
        }

        public void StartLoading()
        {
            var isMetaQuest = SpatialBridge.actorService.localActor.platform == SpatialPlatform.MetaQuest;
            _defaultGroup.SetActive(!isMetaQuest);
            _metaQuestGroup.SetActive(isMetaQuest);

            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }

            StartCoroutine(FadeCoroutine(1f));
        }

        public void StopLoading()
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }

            _alpha = 0f;
            SetAlpha();
            
            _defaultGroup.SetActive(false);
            _metaQuestGroup.SetActive(false);
        }

        private IEnumerator FadeCoroutine(float targetAlpha, float time = .5f)
        {
            var t = 0f;
            while (t < time)
            {
                t += Time.deltaTime;
                _alpha = Mathf.Lerp(_alpha, targetAlpha, t / time);
                SetAlpha();
                yield return null;
            }
            
            _fadeCoroutine = null;
        }

        private void SetAlpha()
        {
            var color = Color.white;
            color.a = _alpha;
            _mpb.SetColor(COLOR_KEY, color);
            _meshRenderer.SetPropertyBlock(_mpb);

            _canvasGroup.alpha = _alpha;
        }
    }
}
