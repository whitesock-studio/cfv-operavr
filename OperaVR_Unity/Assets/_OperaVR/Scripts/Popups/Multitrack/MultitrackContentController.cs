using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    public class MultitrackContentController : MonoBehaviour
    {
        [SerializeField]
        private MultitrackPlayTracker _multitrackPlayTracker;

        [SerializeField]
        private Button _playButton;

        [SerializeField]
        private Button _unpauseButton;

        [SerializeField]
        private Button _pauseButton;

        [SerializeField]
        private bool _startsOn = true;

        [SerializeField]
        private MultitrackTrackView[] _trackViews;

        private MultitrackPopupData _multitrackPopupData;
        private AudioSource _referenceAudioSource;

        private void Awake()
        {
            _multitrackPlayTracker.OnValueChanged += SetAudioSourceTime;
            _playButton.onClick.AddListener(Play);
            _unpauseButton.onClick.AddListener(Unpause);
            _pauseButton.onClick.AddListener(Pause);
        }

        private void LateUpdate()
        {
            var normTime = _referenceAudioSource.time / _referenceAudioSource.clip.length;
            normTime = Mathf.Clamp01(normTime);
            _multitrackPlayTracker.SetValue(normTime);
            SetTracksValue(normTime);

            var isPlaying = _referenceAudioSource.isPlaying;
            var isPaused = !isPlaying && (_referenceAudioSource.time > 0 && 
                _referenceAudioSource.time < _referenceAudioSource.clip.length);
            _playButton.gameObject.SetActive(!isPlaying && !isPaused);
            _unpauseButton.gameObject.SetActive(isPaused);
            _pauseButton.gameObject.SetActive(isPlaying);
        }

        public void LoadData(MultitrackPopupData data)
        {
            _multitrackPopupData = data;
            GetOrInitAudiosource(_multitrackPopupData.TrackClips[0].Clip);

            for (var i = 0; i < _trackViews.Length; i++)
            {
                var trackView = _trackViews[i];

                trackView.IsOn = _startsOn;

                trackView.Stop();
                if (_multitrackPopupData.TrackClips.Length <= i)
                {
                    trackView.gameObject.SetActive(false);
                    continue;
                }

                trackView.gameObject.SetActive(true);
                trackView.LoadClip(_multitrackPopupData.TrackClips[i]);
            }
        }

        private void GetOrInitAudiosource(AudioClip clip)
        {
            _referenceAudioSource = GetComponent<AudioSource>();
            if (_referenceAudioSource == null)
            {
                _referenceAudioSource = gameObject.AddComponent<AudioSource>();
            }
            _referenceAudioSource.clip = clip;
            _referenceAudioSource.volume = 0;
            _referenceAudioSource.Stop();
        }

        private void SetAudioSourceTime(float normalizedTime)
        {
            var targetTime = _referenceAudioSource.clip.length * normalizedTime;
            _referenceAudioSource.time = Mathf.Clamp(targetTime, 0, 
                _referenceAudioSource.clip.length - .01f);
        }

        private void Play()
        {
            _referenceAudioSource.time = 0;
            _referenceAudioSource.Play();

            foreach (var trackView in _trackViews)
            {
                if (!trackView.gameObject.activeSelf)
                {
                    continue;
                }
                trackView.Play();
            }
        }

        private void Unpause()
        {
            _referenceAudioSource.UnPause();
            foreach (var trackView in _trackViews)
            {
                if (!trackView.gameObject.activeSelf)
                {
                    continue;
                }
                trackView.Unpause();
            }
        }

        private void Pause()
        {
            _referenceAudioSource.Pause();
            foreach (var trackView in _trackViews)
            {
                if (!trackView.gameObject.activeSelf)
                {
                    continue;
                }
                trackView.Pause();
            }
        }

        private void SetTracksValue(float normalizedValue)
        {
            foreach (var trackView in _trackViews)
            {
                if (!trackView.gameObject.activeSelf)
                {
                    continue;
                }
                trackView.SetTime(normalizedValue);
            }
        }
    }
}
