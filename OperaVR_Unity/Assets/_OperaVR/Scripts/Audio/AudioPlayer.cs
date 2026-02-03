using UnityEngine;

namespace OperaVR
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _clip;

        public void Play()
        {
            if (_audioSource == null)
            {
                var listener = FindFirstObjectByType<AudioListener>();
                _audioSource = listener.gameObject.GetComponent<AudioSource>();
                if (_audioSource == null)
                {
                    _audioSource = listener.gameObject.AddComponent<AudioSource>();
                }
            }
            if (_clip != null)
            {
                _audioSource.clip = _clip;  
            }
            _audioSource?.Play();
        }
    }
}
