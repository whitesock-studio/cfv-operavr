using UnityEngine;

namespace OperaVR
{
    public class MultitrackContentController : MonoBehaviour
    {
        [SerializeField]
        private MultitrackTrackView[] _trackViews;

        private MultitrackPopupData _multitrackPopupData;

        public void LoadData(MultitrackPopupData data)
        {
            _multitrackPopupData = data;

            for (var i = 0; i < _trackViews.Length; i++)
            {
                var trackView = _trackViews[i];
                trackView.Stop();
                if (_multitrackPopupData.Clips.Length <= i)
                {
                    trackView.gameObject.SetActive(false);
                }

                trackView.gameObject.SetActive(true);
                trackView.LoadClip(_multitrackPopupData.Clips[i]);
            }
        }
    }
}
