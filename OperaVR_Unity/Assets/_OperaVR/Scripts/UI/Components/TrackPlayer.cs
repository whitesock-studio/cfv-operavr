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
            _slider.OnNormalizedTimeRequest += ChangeTimeRequest;
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

        private void ChangeTimeRequest(float normalizedTime)
        {
            _audioSource.time = normalizedTime * _audioSource.clip.length;
        }

        private void ButtonClicked()
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Pause();
                return;
            }
            if (_audioSource.time != 0)
            {
                _audioSource.UnPause();
                return;
            }
            _audioSource.time = 0;
            _audioSource.Play();
        }
    }
}
