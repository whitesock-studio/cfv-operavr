using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    public class TrackPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private CustomImageSlider _slider;

        [SerializeField]
        private Button _playPauseButton;

        [SerializeField]
        private GameObject _playImage;
        
        [SerializeField]
        private GameObject _pauseImage;

        private void Awake()
        {
            _playPauseButton.onClick.AddListener(ButtonClicked);
            _slider.OnNormalizedTimeRequest += SetTime;
        }

        private void Update()
        {
            _slider.SetNormalizedValue(_audioSource.time / _audioSource.clip.length);
            _playImage.SetActive(!_audioSource.isPlaying);
            _pauseImage.SetActive(_audioSource.isPlaying);
        }

        public void LoadClip(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Stop();
        }

        public void Play()
        {
            _audioSource.time = 0;
            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }

        public void Pause()
        {
            _audioSource.Pause();
        }

        public void Unpause()
        {
            _audioSource.UnPause();
        }

        public void SetTime(float normalizedTime)
        {
            _audioSource.time = normalizedTime * _audioSource.clip.length;
        }

        private void ButtonClicked()
        {
            if (_audioSource.isPlaying)
            {
                Pause();
                return;
            }
            if (_audioSource.time != 0)
            {
                Unpause();
                return;
            }
            Play();
        }
    }
}
