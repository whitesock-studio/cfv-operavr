using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    public class MultitrackTrackView : MonoBehaviour
    {
        public TrackPlayer TrackPlayer;

        [SerializeField]
        private TMP_Text _trackNameDisplayer;

        [SerializeField]
        private GameObject _interactabilityMask;

        [SerializeField]
        private Button _toggleButton;

        [SerializeField]
        private ToggleView _toggleView;

        private MultitrackClip _loadedClip;
        public MultitrackClip LoadedClip => _loadedClip;

        private bool _isOn;
        public bool IsOn
        {
            get => _isOn;
            set
            {
                _isOn = value;
                _toggleView.SetOn(_isOn);
                _interactabilityMask.SetActive(!_isOn);
                SetVolume(_isOn ? 1 : 0);
            }
        }

        private void Awake()
        {
            _toggleButton.onClick.AddListener(() => IsOn = !IsOn);
        }

        public void LoadClip(MultitrackClip trackClip)
        {
            _loadedClip = trackClip;
            _trackNameDisplayer.text = _loadedClip.Name;
            TrackPlayer.LoadClip(_loadedClip.Clip);
            Stop();
        }

        public void SetTime(float normalizedTime)
        {
            TrackPlayer.SetTime(normalizedTime);
        }

        public void SetVolume(float volume)
        {
            TrackPlayer.SetVolume(volume);
        }

        public void Play()
        {
            TrackPlayer.Play();
        }

        public void Pause()
        {
            TrackPlayer.Pause();
        }

        public void Unpause()
        {
            TrackPlayer.Unpause();
        }

        public void Stop()
        {
            TrackPlayer.Stop();
        }
    }
}
