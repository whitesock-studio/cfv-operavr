using SpatialSys.UnitySDK;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    public class ResearchContentController : MonoBehaviour
    {
        private enum Page
        {
            Link,
            Code,
            End
        }

        public Action OnComplete;
        public Action OnFail;

        [Header("Link Page")]
        [SerializeField]
        private GameObject _linkPage;

        [SerializeField]
        private Image _linkImage;

        [SerializeField]
        private Button _linkButton;
        
        [SerializeField]
        private Button _toCodelockButton;

        [Header("Codelock Page")]
        [SerializeField]
        private GameObject _codelockPage;

        [SerializeField]
        private Codelock _codelock;

        [SerializeField]
        private Button _backButton;

        [Header("End Page")]
        [SerializeField]
        private GameObject _endPage;

        [SerializeField]
        private Button _endCloseButton;

        private ResearchPopupData _researchPopupData;

        private void Awake()
        {
            _codelock.OnUnlock += () => SetPage(Page.End);
            _codelock.OnError += () => OnFail?.Invoke();
            _linkButton.onClick.AddListener(TryOpenUrl);
            _toCodelockButton.onClick.AddListener(() => SetPage(Page.Code));
            _backButton.onClick.AddListener(() => SetPage(Page.Link));
            _endCloseButton.onClick.AddListener(ResearchCompleted);
        }

        public void LoadData(ResearchPopupData data)
        {
            _researchPopupData = data;
            SetPage(Page.Link);
        }

        public bool CanComplete()
        {
            return _endPage.activeSelf;
        }

        private void TryOpenUrl()
        {
            if (SpatialBridge.actorService.localActor.platform == SpatialPlatform.MetaQuest)
            {
                GUIUtility.systemCopyBuffer = _researchPopupData.Link;
                PopupsManager.Instance.OpenPopup(QuickPopupData.GenerateData(
                    "URL copied:\n" +
                    _researchPopupData.Link));
                return;
            }

            SpatialBridge.spaceService.OpenURL(_researchPopupData.Link);
        }
        
        private void ResearchCompleted()
        {
            OnComplete?.Invoke();
        }

        private void SetPage(Page page)
        {
            _linkPage.SetActive(page == Page.Link);
            _codelockPage.SetActive(page == Page.Code);
            _endPage.SetActive(page == Page.End);

            switch(page)
            {
                case Page.Link:
                    _linkImage.sprite = _researchPopupData.Image;
                    break;
                case Page.Code:
                    _codelock.LoadCodes(_researchPopupData.AcceptedCodes);
                    break;
                case Page.End:
                    break;
            }
        }
    }
}
